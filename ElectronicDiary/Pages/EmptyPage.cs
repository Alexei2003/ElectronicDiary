using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicDiary.Pages
{
    public class EmptyPage : ContentPage
    {
        public EmptyPage()
        {
            Navigation.PushAsync(new AdminPage());
        }
    }
}
