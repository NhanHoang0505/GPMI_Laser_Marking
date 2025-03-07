using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GPMI_Laser_Marking.ViewModels
{
    public class InputValue : ObservableOject
    {
        private static InputValue instance = new InputValue();
        public static InputValue Instance
        {
            get { return instance; }
        }
        private string _PartNumber;
        private string _PartName;
        private string _Version;
        private string _VenderCode;
        private string _OrderNo;
        private string _OrderDate;
        private string _Quantity;
        private string _Pcs;

        //public InputValue()
        //{
        //    //Task.Run(() =>
        //    //{
        //    //    while (true)
        //    //    {
        //    //        PartName = test;
        //    //        Thread.Sleep(500);
        //    //    }
        //    //});
        //    //PartName=test = "b";

        //}
        public string PartNumber { get => _PartNumber; set { _PartNumber = value; OnPropertyChanged(propertyname: "PartNumber"); } }
        public string  PartName { get => _PartName; set { this._PartName = value; OnPropertyChanged("PartName"); } }
        public string Version { get => _Version; set => _Version = value; }
        public string VenderCode { get => _VenderCode; set => _VenderCode = value; }
        public string OrderNo { get => _OrderNo; set => _OrderNo = value; }
        public string OrderDate { get => _OrderDate; set => _OrderDate = value; }
        public string Quantity { get => _Quantity; set => _Quantity = value; }
        public string Pcs { get => _Pcs; set => _Pcs = value; }
        //public override string ToString()
        //{
        //    return NCU + MSoPO + MaSP + ReviewBV;
        //}
    }
}
