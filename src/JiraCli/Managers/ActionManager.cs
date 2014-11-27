// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionManager.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Catel.IoC;
    using Catel.Reflection;

    public class ActionManager : IActionManager
    {
        private readonly Dictionary<string, IAction> _actions = new Dictionary<string, IAction>();

        public ActionManager(ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => typeFactory);

            var types = (from type in TypeCache.GetTypes()
                         where AttributeHelper.IsDecoratedWithAttribute<ActionAttribute>(type)
                         select type);
            foreach (var type in types)
            {
                var actionAttribute = (ActionAttribute)type.GetCustomAttributeEx(typeof (ActionAttribute), false);

                var key = actionAttribute.Name.ToLower();

                var action = (IAction)typeFactory.CreateInstance(type);
                action.Name = actionAttribute.Name;
                action.Description = actionAttribute.Description;
                action.ArgumentsUsage = actionAttribute.ArgumentsUsage;

                _actions[key] = action;
            }
        }

        public IEnumerable<IAction> Actions { get { return _actions.Values; } }

        public IAction GetAction(string name)
        {
            Argument.IsNotNullOrWhitespace(() => name);

            name = name.ToLower();

            return _actions.ContainsKey(name) ? _actions[name] : null;
        }
    }
}