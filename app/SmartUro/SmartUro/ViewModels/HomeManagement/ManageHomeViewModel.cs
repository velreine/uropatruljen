using System;
using System.Collections.Generic;
using System.Text;
using CommonData.Model.Entity;

namespace SmartUro.ViewModels.HomeManagement
{
    public class ManageHomeViewModel
    {
        private Home _home;

        public ManageHomeViewModel() { }

        public ManageHomeViewModel(Home home)
        {
            _home = home;
        }

        public Home Home { get => _home; set => _home = value; }

    }
}
