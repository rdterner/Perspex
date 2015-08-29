﻿// -----------------------------------------------------------------------
// <copyright file="ILayoutable.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Layout
{
    /// <summary>
    /// Defines layout-related functionality for a control.
    /// </summary>
    public interface ILayoutable : IVisual
    {
        /// <summary>
        /// Gets the size that this element computed during the measure pass of the layout process.
        /// </summary>
        Size DesiredSize { get; }

        /// <summary>
        /// Gets the width of the element.
        /// </summary>
        double Width { get; }

        /// <summary>
        /// Gets the height of the element.
        /// </summary>
        double Height { get;  }

        /// <summary>
        /// Gets the minimum width of the element.
        /// </summary>
        double MinWidth { get; }

        /// <summary>
        /// Gets the maximum width of the element.
        /// </summary>
        double MaxWidth { get; }

        /// <summary>
        /// Gets the minimum height of the element.
        /// </summary>
        double MinHeight { get; }

        /// <summary>
        /// Gets the maximum height of the element.
        /// </summary>
        double MaxHeight { get; }

        /// <summary>
        /// Gets the margin around the element.
        /// </summary>
        Thickness Margin { get; }

        /// <summary>
        /// Gets the element's preferred horizontal alignment in its parent.
        /// </summary>
        HorizontalAlignment HorizontalAlignment { get; }

        /// <summary>
        /// Gets the element's preferred vertical alignment in its parent.
        /// </summary>
        VerticalAlignment VerticalAlignment { get; }

        /// <summary>
        /// Gets a value indicating whether the control's layout measure is valid.
        /// </summary>
        bool IsMeasureValid { get; }

        /// <summary>
        /// Gets a value indicating whether the control's layouts arrange is valid.
        /// </summary>
        bool IsArrangeValid { get; }

        /// <summary>
        /// Gets the available size passed in the previous layout pass, if any.
        /// </summary>
        Size? PreviousMeasure { get; }

        /// <summary>
        /// Gets the layout rect passed in the previous layout pass, if any.
        /// </summary>
        Rect? PreviousArrange { get; }

        /// <summary>
        /// Creates the visual children of the control, if necessary
        /// </summary>
        void ApplyTemplate();

        /// <summary>
        /// Carries out a measure of the control.
        /// </summary>
        /// <param name="availableSize">The available size for the control.</param>
        /// <param name="force">
        /// If true, the control will be measured even if <paramref name="availableSize"/> has not
        /// changed from the last measure.
        /// </param>
        void Measure(Size availableSize, bool force = false);

        /// <summary>
        /// Arranges the control and its children.
        /// </summary>
        /// <param name="rect">The control's new bounds.</param>
        /// <param name="force">
        /// If true, the control will be arranged even if <paramref name="rect"/> has not changed
        /// from the last arrange.
        /// </param>
        void Arrange(Rect rect, bool force = false);

        /// <summary>
        /// Invalidates the measurement of the control and queues a new layout pass.
        /// </summary>
        void InvalidateMeasure();

        /// <summary>
        /// Invalidates the arrangement of the control and queues a new layout pass.
        /// </summary>
        void InvalidateArrange();
    }
}