﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BForms.Mvc;
using BForms.Renderers;
using BForms.Models;
using BForms.FormBuilder;

namespace BForms.FormBuilder
{
    public static class FormBuilderExtensions
    {
        public static FormBuilder FormBuilderFor(this HtmlHelper helper)
        {
            return new FormBuilder(helper.ViewContext);
        }
    }

    public class FormBuilder : BsBaseComponent<FormBuilder>
    {
        #region Properties and constructors

        protected BsTheme EditorTheme;
        protected List<FormBuilderControlViewModel> AvailableControls;
        protected List<FormBuilderControl> SelectedControls;
        protected FormBuilderTabsFactory TabsFactory;
        protected ViewContext ViewContext;

        public FormBuilder(ViewContext viewContext)
            : base(viewContext)
        {
            ViewContext = viewContext;

            renderer = new FormEditorBaseRenderer(this);

            EditorTheme = BsTheme.Default;
            AvailableControls = GetDefaultControls();
            TabsFactory = new FormBuilderTabsFactory();
        }

        #endregion

        #region Private methods

        private List<FormBuilderControlViewModel> GetDefaultControls()
        {
            var defaultControls = new List<FormBuilderControlViewModel>();

            defaultControls.Add(new FormBuilderControlViewModel
            {
                Type = FormBuilderControlType.Textbox,
                Glyphicon = Glyphicon.Pencil,
                Text = "Textbox",
                Order = 1,
                TabId = 1
            });

            defaultControls.Add(new FormBuilderControlViewModel
            {
                Type = FormBuilderControlType.Textarea,
                Glyphicon = Glyphicon.Font,
                Text = "Textarea",
                Order = 2,
                TabId = 1
            });

            defaultControls.Add(new FormBuilderControlViewModel
            {
                Type = FormBuilderControlType.NumberPicker,
                Glyphicon = Glyphicon.PlusSign,
                Text = "Number picker",
                Order = 3,
                TabId = 1
            });

            defaultControls.Add(new FormBuilderControlViewModel
            {
                Type = FormBuilderControlType.NumberPickerRange,
                Glyphicon = Glyphicon.PlusSign,
                Text = "Number picker range",
                Order = 4,
                TabId = 1
            });

            defaultControls.Add(new FormBuilderControlViewModel
            {
                Type = FormBuilderControlType.DatePicker,
                Glyphicon = Glyphicon.Calendar,
                Text = "Date picker",
                Order = 5,
                TabId = 1
            });

            defaultControls.Add(new FormBuilderControlViewModel
            {
                Type = FormBuilderControlType.DatePickerRange,
                Glyphicon = Glyphicon.Calendar,
                Text = "Date picker range",
                Order = 6,
                TabId = 1
            });

            defaultControls.Add(new FormBuilderControlViewModel
            {
                Type = FormBuilderControlType.RadioButtonList,
                Glyphicon = Glyphicon.ListAlt,
                Text = "Radio button list",
                Order = 7,
                TabId = 2
            });

            //defaultControls.Add(new FormBuilderControlViewModel
            //{
            //    Type = FormBuilderControlType.Checkbox,
            //    Glyphicon = Glyphicon.Check,
            //    Text = "Checkbox",
            //    Order = 8,
            //    TabId = 2
            //});

            defaultControls.Add(new FormBuilderControlViewModel
            {
                Type = FormBuilderControlType.CheckboxList,
                Glyphicon = Glyphicon.Check,
                Text = "Checkbox list",
                Order = 9,
                TabId = 2
            });

            defaultControls.Add(new FormBuilderControlViewModel
            {
                Type = FormBuilderControlType.SingleSelect,
                Glyphicon = Glyphicon.List,
                Text = "Select list",
                Order = 10,
                TabId = 2
            });

            defaultControls.Add(new FormBuilderControlViewModel
            {
                Type = FormBuilderControlType.ListBox,
                Glyphicon = Glyphicon.Tag,
                Text = "List box",
                Order = 11,
                TabId = 2,
            });

            defaultControls.Add(new FormBuilderControlViewModel
            {
                Type = FormBuilderControlType.TagList,
                Glyphicon = Glyphicon.Tags,
                Text = "Tag list",
                Order = 12,
                TabId = 2,
            });

            defaultControls.Add(new FormBuilderControlViewModel
            {
                Type = FormBuilderControlType.Title,
                Glyphicon = Glyphicon.TextWidth,
                Text = "Title",
                Order = 13,
                TabId = 3,
            });

            defaultControls.Add(new FormBuilderControlViewModel
            {
                Type = FormBuilderControlType.Pagebreak,
                Glyphicon = Glyphicon.LogIn,
                Text = "Pagebreak",
                Order = 14,
                TabId = 3
            });

            return defaultControls;
        }

        #endregion

        #region Public methods

        public FormEditorRenderingOptions GetRenderingOptions()
        {
            return new FormEditorRenderingOptions
            {
                Theme = EditorTheme
            };
        }

        public List<FormBuilderControlViewModel> GetAvailableControls()
        {
            return AvailableControls;
        }

        public List<FormBuilderControl> GetSelectedControls()
        {
            return SelectedControls;
        }

        public ViewContext GetViewContext()
        {
            return ViewContext;
        }

        public List<FormBuilderTabBuilder> GetTabs()
        {
            return TabsFactory.GetTabs();
        }

        #endregion

        #region Fluent methods

        public FormBuilder ConfigureControls()
        {
            return this;
        }

        public FormBuilder ConfigureTabs(Action<FormBuilderTabsFactory> action)
        {
            action(TabsFactory);

            return this;
        }

        #endregion
    }

    #region Helper classes and enums

    public class FormEditorRenderingOptions
    {
        public BsTheme Theme { get; set; }
    }

    #endregion
}
