﻿// Copyright (c) The Perspex Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Perspex.Interactivity;
using Perspex.VisualTree;

namespace Perspex.Input
{
    /// <summary>
    /// Handles access keys for a window.
    /// </summary>
    public class AccessKeyHandler : IAccessKeyHandler
    {
        /// <summary>
        /// Defines the AccessKeyPressed attached event.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> AccessKeyPressedEvent =
            RoutedEvent.Register<RoutedEventArgs>(
                "AccessKeyPressed",
                RoutingStrategies.Bubble,
                typeof(AccessKeyHandler));

        /// <summary>
        /// The registered access keys.
        /// </summary>
        private readonly List<Tuple<string, IInputElement>> _registered = new List<Tuple<string, IInputElement>>();

        /// <summary>
        /// The window to which the handler belongs.
        /// </summary>
        private IInputRoot _owner;

        /// <summary>
        /// Whether access keys are currently being shown;
        /// </summary>
        private bool _showingAccessKeys;

        /// <summary>
        /// Whether to ignore the Alt KeyUp event.
        /// </summary>
        private bool _ignoreAltUp;

        /// <summary>
        /// Gets or sets the window's main menu.
        /// </summary>
        public IMainMenu MainMenu { get; set; }

        /// <summary>
        /// Sets the owner of the access key handler.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <remarks>
        /// This method can only be called once, typically by the owner itself on creation.
        /// </remarks>
        public void SetOwner(IInputRoot owner)
        {
            Contract.Requires<ArgumentNullException>(owner != null);

            if (_owner != null)
            {
                throw new InvalidOperationException("AccessKeyHandler owner has already been set.");
            }

            _owner = owner;

            _owner.AddHandler(InputElement.KeyDownEvent, OnPreviewKeyDown, RoutingStrategies.Tunnel);
            _owner.AddHandler(InputElement.KeyDownEvent, OnKeyDown, RoutingStrategies.Bubble);
            _owner.AddHandler(InputElement.KeyUpEvent, OnPreviewKeyUp, RoutingStrategies.Tunnel);
            _owner.AddHandler(InputElement.PointerPressedEvent, OnPreviewPointerPressed, RoutingStrategies.Tunnel);
        }

        /// <summary>
        /// Registers an input element to be associated with an access key.
        /// </summary>
        /// <param name="accessKey">The access key.</param>
        /// <param name="element">The input element.</param>
        public void Register(char accessKey, IInputElement element)
        {
            var existing = _registered.FirstOrDefault(x => x.Item2 == element);

            if (existing != null)
            {
                _registered.Remove(existing);
            }

            _registered.Add(Tuple.Create(accessKey.ToString().ToUpper(), element));
        }

        /// <summary>
        /// Unregisters the access keys associated with the input element.
        /// </summary>
        /// <param name="element">The input element.</param>
        public void Unregister(IInputElement element)
        {
            foreach (var i in _registered.Where(x => x.Item2 == element).ToList())
            {
                _registered.Remove(i);
            }
        }

        /// <summary>
        /// Called when a key is pressed in the owner window.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftAlt)
            {
                if (MainMenu == null || !MainMenu.IsOpen)
                {
                    // When Alt is pressed without a main menu, or with a closed main menu, show
                    // access key markers in the window (i.e. "_File").
                    _owner.ShowAccessKeys = _showingAccessKeys = true;
                }
                else
                {
                    // If the Alt key is pressed and the main menu is open, close the main menu.
                    CloseMenu();
                    _ignoreAltUp = true;
                }

                // We always handle the Alt key.
                e.Handled = true;
            }
        }

        /// <summary>
        /// Called when a key is pressed in the owner window.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnKeyDown(object sender, KeyEventArgs e)
        {
            bool menuIsOpen = MainMenu?.IsOpen == true;

            if (e.Key == Key.Escape && menuIsOpen)
            {
                // When the Escape key is pressed with the main menu open, close it.
                CloseMenu();
                e.Handled = true;
            }
            else if ((e.Modifiers & InputModifiers.Alt) != 0 || menuIsOpen)
            {
                // If any other key is pressed with the Alt key held down, or the main menu is open,
                // find all controls who have registered that access key.
                var text = e.Key.ToString().ToUpper();
                var matches = _registered
                    .Where(x => x.Item1 == text && x.Item2.IsEffectivelyVisible)
                    .Select(x => x.Item2);

                // If the menu is open, only match controls in the menu's visual tree.
                if (menuIsOpen)
                {
                    matches = matches.Where(x => MainMenu.IsVisualAncestorOf(x));
                }

                var match = matches.FirstOrDefault();

                // If there was a match, raise the AccessKeyPressed event on it.
                if (match != null)
                {
                    match.RaiseEvent(new RoutedEventArgs(AccessKeyPressedEvent));
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Handles the Alt/F10 keys being released in the window.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftAlt:
                    if (_ignoreAltUp)
                    {
                        _ignoreAltUp = false;
                    }
                    else if (_showingAccessKeys && MainMenu != null)
                    {
                        MainMenu.Open();
                        e.Handled = true;
                    }

                    break;

                case Key.F10:
                    _owner.ShowAccessKeys = _showingAccessKeys = true;
                    MainMenu.Open();
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// Handles pointer presses in the window.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnPreviewPointerPressed(object sender, PointerEventArgs e)
        {
            if (_showingAccessKeys)
            {
                _owner.ShowAccessKeys = false;
            }
        }

        /// <summary>
        /// Closes the <see cref="MainMenu"/> and performs other bookeeping.
        /// </summary>
        private void CloseMenu()
        {
            MainMenu.Close();
            _owner.ShowAccessKeys = _showingAccessKeys = false;
        }
    }
}
