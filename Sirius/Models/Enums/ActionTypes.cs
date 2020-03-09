using System;
using System.Collections.Generic;

namespace Sirius.Models.Enums
{
    public static class Actions
    {
        public static string GetActionAliasByType(string type)
        {
            var result = new List<Action> {
                new Action("CREATED", "Создано"),
                new Action("DELETED", "Удалено"),
                new Action("UPDATED", "Обновлено"),
                new Action("GET", "Просмотр")
            }.Find(item => item.Type == type).Alias;
            return result;
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