using System;
using System.Collections.Generic;
using System.Text;

namespace FirstApp.Model
{
    public class CheckBox<T>
    {
        public T Data { get; set; }
        public bool Selected { get; set; }
    }
}
