﻿using BForms.Models;
using BForms.Mvc;
using BForms.Renderers;
using BForms.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BForms.Editor
{
    public class BsEditorGroupConfigurator<TModel> : BsBaseConfigurator
    {
        #region Properties and Constructor
        internal Dictionary<object, BsEditorGroupBuilder> Groups { get; set; }
        internal List<TabGroupConnection> Connections { get; set; }
        internal string FormHtml { get; set; }

        public BsEditorGroupConfigurator(ViewContext viewContext) : base(viewContext)
        {
            this.Groups = new Dictionary<object, BsEditorGroupBuilder>();
            this.Connections = new List<TabGroupConnection>();
        }

        public string Title { get; set; }
        #endregion

        #region Public Methods
        public BsEditorGroupBuilder<TEditor> For<TEditor>(Expression<Func<TModel, TEditor>> expression)
            where TEditor : IBsEditorGroupModel
        {
            BsEditorGroupBuilder builder;

            builder = this.GetGroup(expression);

            return builder as BsEditorGroupBuilder<TEditor>;
        }

        public BsEditorGroupConfigurator<TModel> FormTemplate(MvcHtmlString template)
        {
            this.FormHtml = template.ToString();

            return this;
        }
        #endregion

        #region Helpers
        private BsEditorGroupBuilder GetGroup<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            var prop = expression.GetPropertyInfo<TModel, TValue>();

            BsEditorGroupAttribute attr = null;

            if (ReflectionHelpers.TryGetAttribute(prop, out attr))
            {
                var id = attr.Id;

                return this.Groups[id];
            }

            throw new Exception("Property " + prop.Name + " has no BsGroupEditorAttribute");
        }

        private void Add<TEditor, TRow>(BsEditorGroupAttribute attr, IBsEditorGroupModel model, object[] editableTabIds, string propertyName) 
            where TEditor : IBsEditorGroupModel
            where TRow : BsEditorGroupItemModel, new()
        {

            var group = new BsEditorGroupBuilder<TEditor>(model, this.viewContext, editableTabIds)
                       .Id(attr.Id);

            group.SetPropertyName(propertyName);

            var rowType = typeof(TRow);
            
            var genericArgs = rowType.BaseType.GetGenericArguments();

            if (genericArgs.Count() == 1) // register renderer via reflection because we don't know the type of TForm
            {
                var method = typeof(BsEditorGroupBuilder<TEditor>).GetMethod("RegisterRenderer", 
                    BindingFlags.Default | BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic);
                var generic = method.MakeGenericMethod(typeof(TRow), genericArgs[0]); // genericArgs[0] => TForm
                generic.Invoke(group, null);
            }
            else
            {
                group.renderer = new BsEditorGroupRenderer<TEditor, TRow>(group);
            }

            if (model != null)
            {
                var connection = model.GetTabGroupConnection();

                if (connection != null)
                {
                    connection.GroupId = attr.Id;
                    this.Connections.Add(connection);
                }
            }

            InsertGroup<TEditor, TRow>(attr.Id, group);
        }

        private void InsertGroup<TEditor, TRow>(object id, BsEditorGroupBuilder<TEditor> tabBuilder)
            where TEditor : IBsEditorGroupModel
            where TRow : BsEditorGroupItemModel
        {
            this.Groups.Add(id, tabBuilder);
        }

        internal object[] GetGroupIds()
        {
            return this.Groups.Select(x => x.Value.Uid).Reverse().ToArray();
        }
        #endregion
    }
}
