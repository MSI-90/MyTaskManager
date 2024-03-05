using DataLayer.DTO;
using MyTaskManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServiceLayer.Helpers
{
    public class TaskHelper
    {
        //PriorityFrom priority;
        private string DescriptionVariants(byte id)
        {
            string[] descriptionVariants =
                [
                    "обычно относится к задачам или действиям, которые имеют низкую степень важности или срочности. " +
                    "Это могут быть задачи, которые могут быть выполнены во второстепенном порядке или отложены в пользу более важных задач.",

                    "обычно относится к задачам или действиям, которые имеют среднюю степень важности или срочности. " +
                    "Это могут быть задачи, которые требуют выполнения в разумные сроки, но не являются критически важными.",

                    "обычно относится к задачам или действиям, которые имеют высокую степень важности или срочности. " +
                    "Это могут быть критически важные задачи, которые требуют немедленного внимания и выполнения."
                ];

            return descriptionVariants[id];
        }

        public string GetPriorityDescription(PriorityFrom priority)
        {
            return DescriptionVariants((byte)priority);
        }
    }
}
