using Microsoft.AspNetCore.Components;

namespace ItTechnologies.CoreUI.Models
{
    public class NavMenuItem
    {
        public NavMenuItem(string text, string href, Type icon)
        {
            Text = text;
            Href = href;
            Icon = builder =>
            {
                builder.OpenComponent(0, icon);
                builder.CloseComponent();
            };
        }

        public string Href { get; set; }
        public RenderFragment Icon { get; set; }
        public string Text { get; set; }
    }
}