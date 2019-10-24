using System;
using System.Collections.Generic;

namespace MainProject.Framework.Models.DynamicForm
{
    public class Form
    {
        public string type { get; set; }

        public string label { get; set; }

        public string className { get; set; }

        public string name { get; set; }

        public bool required { get; set; }

        // Button: button, submit, reset
        // Text: text, email, password, tel, ...
        public string subtype { get; set; }

        public string placeholder { get; set; }

        public string description { get; set; }

        public int maxlength { get; set; }

        // Text Area
        public int rows { get; set; }

        // CheckBox
        public bool inline { get; set; }

        public List<RadioCheckBox> values { get; set; }

        // DateTime and Number Box use this field
        public string value { get; set; }

        // Number

        public int min { get; set; }

        public int max { get; set; }
    }

    // RadioBox, CheckBox, SelectOptionBox
    public class RadioCheckBox
    {
        public string label { get; set; }

        public string value { get; set; }

        public bool selected { get; set; }
    }
}
