using System;
using System.Collections.Generic;
using System.Text;

namespace FirstApp.Model
{
    public class Translator
    {
        //Dodać wybór język na jaki chcemy przetłumaczyć aplikację
        static Dictionary<string, string> translations = new Dictionary<string, string>
        {
            {"Habit", "Nawyk"},
            {"Habit Plan", "Plan Nawyku"},
            {"Start Date", "Data Początkowa"},
            {"End Date", "Data Końca"},
            {"Description", "Opis"}
        };
        public string Translate(string WordToTranslate)
        {
            translations.TryGetValue(translations[WordToTranslate], out string result);
            if(result == null)
            {
                return WordToTranslate;
            }
            else
            {
                return result;
            }
        }

    }
}
