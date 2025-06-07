using ServerApp2.Models;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace ClientApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 5001))
                {
                    NetworkStream stream = client.GetStream();
                    Request request = new Request();
                    request.Action = "view";
                    // TODO
                    // request -> string
                    string messageRequest = JsonSerializer.Serialize(request);

                    // string -> byte
                    byte[] bufferRequest = Encoding.UTF8.GetBytes(messageRequest);

                    // write byte on stream
                    stream.Write(bufferRequest, 0, bufferRequest.Length);

                    // TODO:
                    // byte -> string
                    byte[] buffer = new byte[8000];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    client.Close();
                    
                    string message = Encoding.UTF8.GetString(buffer, 0, bytes);

                    // string -> Response Object
                    var data = JsonSerializer.Deserialize<Response>(message);

                    dgProduct.ItemsSource = data.listProducts
                        .Select(p => new
                        {
                            p.ProductId,
                            p.ProductName,
                            p.UnitPrice,
                            p.UnitsInStock,
                            p.Image,
                            p.CategoryId,
                            p.Category?.CategoryName
                        })
                        .ToList();
                    cbCate.ItemsSource = data.listCateNames;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Client fail: " + ex.Message);
            }
        }


        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            product.ProductName = "AppleVision";
            product.UnitPrice = 10000;
            product.UnitsInStock = 500;
            product.Image = "abc.pneg";
            product.CategoryId = 1;

            Request req = new Request();
            req.Action = "add";
            req.ObjectName = "product";
            req.Data = product;

            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 5001))
                {
                    NetworkStream stream = client.GetStream();

                    // TODO
                    // request -> string
                    string message = JsonSerializer.Serialize(req);

                    // string -> byte
                    byte[] buffer = Encoding.UTF8.GetBytes(message);

                    // write byte on stream
                    stream.Write(buffer, 0, buffer.Length);
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Client fail: " + ex.Message);
            }


        }

        // Nhan nut LOAD DATA, thi se connect sang Server de lay danh sach Product va
        // hien thi len DataGrid
    }
}