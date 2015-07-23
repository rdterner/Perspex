﻿// -----------------------------------------------------------------------
// <copyright file="StyleSelector.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Styling
{
    using System;
    using System.Collections.Generic;

    public class StyleSelector
    {
        private Func<IStyleable, SelectorMatch> evaluate;

        private bool inTemplate;

        private bool stopTraversal;

        private string description;

        public StyleSelector()
        {
            this.evaluate = _ => SelectorMatch.True;
        }

        public StyleSelector(
            StyleSelector previous,
            Func<IStyleable, SelectorMatch> evaluate,
            string selectorString,
            bool inTemplate = false,
            bool stopTraversal = false)
            : this()
        {
            Contract.Requires<ArgumentNullException>(previous != null);

            this.Previous = previous;
            this.evaluate = evaluate;
            this.SelectorString = selectorString;
            this.inTemplate = inTemplate || previous.inTemplate;
            this.stopTraversal = stopTraversal;
        }

        public StyleSelector Previous
        {
            get;
            private set;
        }

        public string SelectorString
        {
            get;
            set;
        }

        public StyleSelector MovePrevious()
        {
            return this.stopTraversal ? null : this.Previous;
        }

        public SelectorMatch Match(IStyleable control)
        {
            List<IObservable<bool>> inputs = new List<IObservable<bool>>();
            StyleSelector selector = this;

            while (selector != null)
            {
                var templatedParent = control.GetValue(StyleSelectors.TemplatedParentProperty);

                if (selector.inTemplate && templatedParent == null)
                {
                    return SelectorMatch.False;
                }

                var match = selector.evaluate(control);

                if (match.ImmediateResult == false)
                {
                    return match;
                }
                else if (match.ObservableResult != null)
                {
                    inputs.Add(match.ObservableResult);
                }

                selector = selector.MovePrevious();
            }

            if (inputs.Count > 0)
            {
                return new SelectorMatch(new StyleActivator(inputs));
            }
            else
            {
                return SelectorMatch.True;
            }
        }

        public override string ToString()
        {
            if (this.description == null)
            {
                string result = string.Empty;

                if (this.Previous != null)
                {
                    result = this.Previous.ToString();
                }

                this.description = result + this.SelectorString;
            }

            return this.description;
        }
    }
}
