using System;
using System.Collections.Generic;

namespace Sirius.Models.Enums
{
    public class Actions
    {
        public IEnumerable<Action> ActionTypes()
        {
            return new Action[] {
                new Action("CREATED", "Создано"),
                new Action("DELETED", "Удалено"),
                new Action("UPDATED", "Обновлено")
            };
        }
    }

    public class Action
    {
        public string Type { get; set; }
        public string Alias { get; set; }
        public Action(string type, string alias)
        {
            Type = type;
            Alias = alias;
        }
    }
}