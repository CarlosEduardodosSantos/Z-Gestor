using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using FastReport;
using FastReport.Utils;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Componente
{
    public class ImprimirComprovante
    {
        private readonly PedidoRootModel _pedido;

        public ImprimirComprovante(PedidoRootModel pedido)
        {
            _pedido = pedido;
        }

        public void Imprimir(int tipoOp)
        {
            var report = new Report();
            /*
            var ds = new DataSet();

            ds.Tables.Add(ConverteObjetoParaDataTable(_pedido, "pedido"));
            ds.Tables.Add(ConverteListParaDataTable(_pedido.items, "pedidoItens"));
            ds.Tables.Add(ConverteObjetoParaDataTable(_pedido.customer, "cliente"));
            ds.Tables.Add(ConverteObjetoParaDataTable(_pedido.deliveryAddress, "enderecoEntrega"));
            ds.Tables.Add(ConverteListParaDataTable(_pedido.payments, "formaPagamentos"));
            */
            //_pedido.qrcodePix = $"00020126360014BR.GOV.BCB.PIX0114+551698835104152040000530398654071000.005802BR5901A6008RIBEIRAOPRETO62120508pedido:{_pedido.vendaId}63041BB9";
            var pedidoSource = new List<PedidoRootModel>();
            foreach (var pedidoItem in _pedido.items)
            {
                if (pedidoItem.options == null)
                    pedidoItem.subItems = new List<Subitem>();
                /*
                if (pedidoItem.subItems.Count == 0)
                    pedidoItem.subItems.Add(new Subitem());*/
            }
            _pedido.delivery = _pedido.delivery ?? new Delivery();
            pedidoSource.Add(_pedido);

            report.RegisterData(pedidoSource, "pedidos");

            var reportPropriedade = new EnvironmentSettings
            {
                PreviewSettings = { ShowInTaskbar = false },
                UIStyle = UIStyle.VisualStudio2005
            };

            reportPropriedade.PreviewSettings.Buttons = PreviewButtons.Close |
                                                        PreviewButtons.Print |
                                                        PreviewButtons.Zoom |
                                                        PreviewButtons.Save;

            var cupomFrx = $"{Environment.CurrentDirectory}\\Cupom_{_pedido.aplicacao.ToString()}.frx";
            if (File.Exists(cupomFrx))
            {
                report.Load(cupomFrx);
                report.Prepare();
            }
            else
                tipoOp = 2;


            switch (tipoOp)
            {
                case 0:
                    {
                        report.Show();
                        break;
                    }
                case 1:
                    {
                        report.PrintSettings.ShowDialog = false;
                        report.Print();
                        break;
                    }
                case 2:
                    {
                        report.Design();
                        break;
                    }
            }
        }

        public static DataTable ConverteListParaDataTable<T>(List<T> list, string tableNome)
        {
            DataTable dt = new DataTable(tableNome);

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                Type pt = info.PropertyType;
                if (pt.IsGenericType && pt.GetGenericTypeDefinition() == typeof(Nullable<>))
                    pt = Nullable.GetUnderlyingType(pt);

                dt.Columns.Add(new DataColumn(info.Name, pt));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    object value = info.GetValue(t, null);
                    if (value != null)
                        row[info.Name] = value;

                    //row[info.Name] = info.GetValue(t, null);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static DataTable ConverteObjetoParaDataTable<T>(T obj, string tableNome)
        {
            DataTable dt = new DataTable(tableNome);

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                Type pt = info.PropertyType;
                if (pt.IsGenericType && pt.GetGenericTypeDefinition() == typeof(Nullable<>))
                    pt = Nullable.GetUnderlyingType(pt);

                dt.Columns.Add(new DataColumn(info.Name, pt));
            }

            DataRow row = dt.NewRow();
            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                object value = info.GetValue(obj, null);
                if (value != null)
                    row[info.Name] = value;

                //row[info.Name] = info.GetValue(t, null);
            }
            dt.Rows.Add(row);

            return dt;
        }


    }
}