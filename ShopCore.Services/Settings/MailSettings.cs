namespace ShopCore.Services.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ShopCore.Services.Interfaces;

    public class MailSettings
    {
        public string From { get; set; }

        public string SmtpServer { get; set; }

        public int Port { get; set; }

        // TODO няма логика да използваш конкретното име на темплейт,
        // Ако утре се наложи да пращаме email за успешна регистрация и смяна на парола, какво ще правиш?
        // Помисли да пазиш име с директория в която се съхраняват различни шаблони (в случая само 1 шаблон)
        // Освено това можеш да добавиш един enum TemplateType { Receipt = 1, ...}
        // и после направи метод който от TemplateFolder и TemplateType да връша стринг с пътя за поискания шаблон
        public string TemplatePath { get; set; }
    }
}
