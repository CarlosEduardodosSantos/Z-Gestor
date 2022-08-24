using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ZapFood.WinForm
{
    public class Funcoes
    {
        public static bool Val_NumeroKey(string _text)
        {
            if (string.IsNullOrEmpty(_text)) return false;
            Regex er = new Regex("^[0-9,\b8]");
            if (er.Match(_text).Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static IList Listar(Type tipo)
        {
            ArrayList lista = new ArrayList();
            if (tipo != null)
            {
                Array enumValores = Enum.GetValues(tipo);
                foreach (Enum valor in enumValores)
                {
                    lista.Add(new KeyValuePair<int, String>(Convert.ToInt32(valor), ObterDescricao(valor)));
                }
            }

            return lista;
        }
        public static String ObterDescricao(Enum valor)
        {
            FieldInfo fieldInfo = valor.GetType().GetField(valor.ToString());

            DescriptionAttribute[] atributos = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return atributos.Length > 0 ? atributos[0].Description ?? "Nulo" : valor.ToString();
        }

        public static OpenFileDialog BuscarImagem()
        {
            return BuscarArquivo("Selecione uma imagem", "JPG", "PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|JPEG (*.jpeg)|*.jpeg|JPG (*.jpg)|*.jpg|GIF (*.gif)|*.gif");
        }
        public static OpenFileDialog BuscarArquivo(string titulo, string extensaoPadrao, string filtro, string arquivoPadrao = null)
        {
            var dlg = new OpenFileDialog
            {
                Title = titulo,
                FileName = arquivoPadrao,
                DefaultExt = extensaoPadrao,
                Filter = filtro
            };
            dlg.ShowDialog();
            return dlg;
        }
        public static string ConvertDateTimeFuso(string strData)
        {
            var dt = DateTime.Parse(strData);

            return String.Format("{0:s}{0:zzz}", dt);
        }
        public static  string SoNumeros(string value)
        {
            return string.IsNullOrEmpty(value) ? String.Empty : String.Join("", Regex.Split(value, @"[^\d]"));
        }

        /// <summary>
        ///     Exibe um diálogo com uma mensagem para o usuário, utilizando um ModernDialog
        /// </summary>
        /// <param name="mensagem"></param>
        /// <param name="titulo"></param>
        /// <param name="botoes"></param>
        /// <param name="imagem"></param>
        public static void Mensagem(string mensagem, string titulo, MessageBoxButtons botoes, MessageBoxIcon imagem = MessageBoxIcon.None)
        {
            MessageBox.Show(mensagem, titulo, botoes, imagem);
        }


        public static void reduzir(string caminhoArquivoOriginal, string caminhoArquivoDestino, long qualidade)
        {
            Bitmap myBitmap;
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            // Create a Bitmap object based on a BMP file.
            myBitmap = new Bitmap(caminhoArquivoOriginal);

            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            // Create an Encoder object based on the GUID

            // for the Quality parameter category.
            myEncoder = System.Drawing.Imaging.Encoder.Quality;

            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);

            // Save the bitmap as a JPEG file with quality level 25.            
            myEncoderParameter = new EncoderParameter(myEncoder, qualidade);
            myEncoderParameters.Param[0] = myEncoderParameter;


            myBitmap.Save(caminhoArquivoDestino, myImageCodecInfo, myEncoderParameters);
            myBitmap.Dispose();
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

    }
}