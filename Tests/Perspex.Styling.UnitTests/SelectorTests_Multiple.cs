﻿// -----------------------------------------------------------------------
// <copyright file="SelectorTests_Multiple.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Styling.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Perspex.Controls.Core;
    using Perspex.Controls.Standard;
    using Perspex.Styling;
    using Xunit;

    public class SelectorTests_Multiple
    {
        [Fact]
        public void Template_Child_Of_Control_With_Two_Classes()
        {
            Assert.True(false);
            ////var template = new LooklessControlTemplate(parent =>
            ////{
            ////    return new Border
            ////    {
            ////        Name = "border",
            ////    };
            ////});

            ////var control = new Button
            ////{
            ////    Template = template,
            ////};

            ////control.ApplyTemplate();

            ////var selector = new StyleSelector()
            ////    .OfType<Button>()
            ////    .Class("foo")
            ////    .Class("bar")
            ////    .Template()
            ////    .Name("border");

            ////var border = (Border)((IVisual)control).VisualChildren.Single();
            ////var values = new List<bool>();
            ////var activator = selector.Match(border).ObservableResult;

            ////activator.Subscribe(x => values.Add(x));

            ////Assert.Equal(new[] { false }, values);
            ////control.Classes.Add("foo", "bar");
            ////Assert.Equal(new[] { false, true }, values);
            ////control.Classes.Remove("foo");
            ////Assert.Equal(new[] { false, true, false }, values);
        }
    }
}
