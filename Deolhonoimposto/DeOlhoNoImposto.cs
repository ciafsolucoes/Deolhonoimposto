using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;


namespace Deolhonoimposto
{
    class DeOlhoNoImposto
    {
        protected string cnpj;
        protected string token;


        [JsonProperty(PropertyName = "Codigo")]
        public int Codigo { get; set; }

        [JsonProperty(PropertyName = "UF")]
        public string UF { get; set; }

        [JsonProperty(PropertyName = "EX")]
        public string EX { get; set; }

        [JsonProperty(PropertyName = "Descricao")]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "Nacional")]
        public decimal Nacional { get; set; }

        [JsonProperty(PropertyName = "Estadual")]
        public decimal Estadual { get; set; }

        [JsonProperty(PropertyName = "Importado")]
        public decimal Importado { get; set; }

        [JsonProperty(PropertyName = "Municipal")]
        public decimal Municipal { get; set; }

        [JsonProperty(PropertyName = "Tipo")]
        public string Tipo { get; set; }

        [JsonProperty(PropertyName = "VigenciaInicio")]
        public string VigenciaInicio { get; set; }

        [JsonProperty(PropertyName = "VigenciaFim")]
        public string VigenciaFim { get; set; }

        [JsonProperty(PropertyName = "Chave")]
        public string Chave { get; set; }

        [JsonProperty(PropertyName = "Versao")]
        public string Versao { get; set; }

        [JsonProperty(PropertyName = "Fonte")]
        public string Fonte { get; set; }

        [JsonProperty(PropertyName = "Valor")]
        public decimal Valor { get; set; }

        [JsonProperty(PropertyName = "ValorTributoNacional")]
        public decimal ValorTributoNacional { get; set; }

        [JsonProperty(PropertyName = "ValorTributoEstadual")]
        public decimal ValorTributoEstadual { get; set; }

        [JsonProperty(PropertyName = "ValorTributoImportado")]
        public decimal ValorTributoImportado { get; set; }

        [JsonProperty(PropertyName = "ValorTributoMunicipal")]
        public decimal ValorTributoMunicipal { get; set; }


        public DeOlhoNoImposto(string cnpj, string token) {
            this.cnpj = cnpj;
            this.token = token;
        }

        public decimal GetValorAproxTributos()
        {
            decimal vlaproxtrib = 0;
            try
            {
                //+ ValorTributoImportado
                vlaproxtrib = ValorTributoNacional + ValorTributoEstadual + ValorTributoMunicipal;
                return vlaproxtrib;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                return vlaproxtrib;
            }
        }

        public DeOlhoNoImposto ConsultarProduto(string NCM, string valor, string uf, string ex, string descricao, string unidadeMedida, string gtin)
        {
            DeOlhoNoImposto novo = new DeOlhoNoImposto(cnpj, token);
            try
            {
                string token = "Lvl6gv_CyOrnRp1DtJiy5WmONpVG3SMY31lWuDlGnByriEbPBV39FUVrsnQ1Nhkn";

                string UrlRequisicao = @"https://apidoni.ibpt.org.br/api/v1/produtos?token=" + token;
                UrlRequisicao += @"&cnpj=" + cnpj;
                UrlRequisicao = UrlRequisicao + @"&codigo=" + NCM;
                UrlRequisicao = UrlRequisicao + @"&uf=" + uf;

                if (string.IsNullOrWhiteSpace(ex))
                    UrlRequisicao += @"&ex=0";
                else UrlRequisicao += @"&ex=" + ex.Replace(" ", "");

                if (string.IsNullOrWhiteSpace(descricao))
                    UrlRequisicao += @"&descricao=SEMDESCRICAO";
                else UrlRequisicao += @"&descricao=" + descricao.Replace(" ", "");

                if (string.IsNullOrWhiteSpace(unidadeMedida))
                    UrlRequisicao += @"&unidadeMedida=UN";
                else UrlRequisicao += @"&unidadeMedida=" + unidadeMedida.Replace(" ", "");


                UrlRequisicao = UrlRequisicao + @"&valor=" + valor.Replace(".", "").Replace(",", ".");

                if (string.IsNullOrWhiteSpace(gtin))
                    UrlRequisicao += @"&gtin=12345678";
                else UrlRequisicao += @"&gtin=" + gtin.Replace(" ", "");

                const string _mediaType = "application/json";
                const string _charSet = "UTF-8";


                var req = (HttpWebRequest)WebRequest.Create(UrlRequisicao);
                req.Method = "GET";
                req.ContentType = _mediaType + ";charset=" + _charSet;
                // req.ContentLength = data.Length;
                req.Accept = _mediaType;
                req.Headers.Add(HttpRequestHeader.AcceptCharset, _charSet);

                var response = (HttpWebResponse)req.GetResponse();
                var bodyResposta = response.GetResponseStream();
                using (var reader = new StreamReader(bodyResposta))
                {
                    var jsonretorno = reader.ReadToEnd().Replace("null", "0");
                    novo = JsonConvert.DeserializeObject<DeOlhoNoImposto>(jsonretorno);
                }
                return novo;
            }
            catch (Exception erro)
            {
                Console.WriteLine("\nMessage ---\n{0}", erro.Message);
                return novo;
            }
        }

        public DeOlhoNoImposto ConsultarServico(string codigo, string uf, string descricao, string unidadeMedida, string valor)
        {
            DeOlhoNoImposto novo = new DeOlhoNoImposto(cnpj, token);
            try
            {
                string UrlRequisicao = @"https://apidoni.ibpt.org.br/api/v1/servicos?token=" + token;
                UrlRequisicao += @"&cnpj=" + cnpj;
                UrlRequisicao = UrlRequisicao + @"&codigo=" + codigo;
                UrlRequisicao = UrlRequisicao + @"&uf=" + uf;

                if (string.IsNullOrWhiteSpace(descricao))
                    UrlRequisicao += @"&descricao=SEMDESCRICAO";
                else UrlRequisicao += @"&descricao=" + descricao.Replace(" ", "");

                if (string.IsNullOrWhiteSpace(unidadeMedida))
                    UrlRequisicao += @"&unidadeMedida=UN";
                else UrlRequisicao += @"&unidadeMedida=" + unidadeMedida.Replace(" ", "");

                UrlRequisicao = UrlRequisicao + @"&valor=" + valor.Replace(".", "").Replace(",", ".");

                const string _mediaType = "application/json";
                const string _charSet = "UTF-8";


                var req = (HttpWebRequest)WebRequest.Create(UrlRequisicao);
                req.Method = "GET";
                req.ContentType = _mediaType + ";charset=" + _charSet;                
                req.Accept = _mediaType;
                req.Headers.Add(HttpRequestHeader.AcceptCharset, _charSet);

                var response = (HttpWebResponse)req.GetResponse();
                var bodyResposta = response.GetResponseStream();
                using (var reader = new StreamReader(bodyResposta))
                {
                    var jsonretorno = reader.ReadToEnd().Replace("null", "0");
                    novo = JsonConvert.DeserializeObject<DeOlhoNoImposto>(jsonretorno);
                }
                return novo;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                return novo;
            }
        }



    }
}
