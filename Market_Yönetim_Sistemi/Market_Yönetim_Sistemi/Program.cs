using System;
using System.Collections.Generic;

namespace Market_Yönetim_Sistemi
{
    class Program
    {
        static void Main(string[] args)
        {
            Market market = new Market();
            market.Initialize();

            Console.WriteLine("Market Yönetim Sistemine Hoş Geldiniz!");
            Console.WriteLine("1- Admin Girişi");
            Console.WriteLine("2- Müşteri Girişi");
            Console.Write("Seçiminiz: ");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || (choice < 1 || choice > 2))
            {
                Console.Write("Geçerli bir seçim yapınız (1 veya 2): ");
            }

            if (choice == 1)
            {
                market.AdminPanel();
            }
            else
            {
                market.CustomerPanel();
            }
        }
    }

    public class Market
    {
        private List<Product> products = new List<Product>();
        private List<Order> orders = new List<Order>();

        public void Initialize()
        {
            products.Add(new Product("Elma", 10, 5.0m));
            products.Add(new Product("Süt", 20, 12.5m));
            products.Add(new Product("Ekmek", 15, 3.0m));
        }

        public void AdminPanel()
        {
            Console.Clear();
            Console.WriteLine("Admin Paneline Hoş Geldiniz!");
            while (true)
            {
                Console.WriteLine("\n1- Ürün Ekle");
                Console.WriteLine("2- Ürün Listele");
                Console.WriteLine("3- Siparişleri Görüntüle");
                Console.WriteLine("4- Çıkış");
                Console.Write("Seçiminiz: ");

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || (choice < 1 || choice > 4))
                {
                    Console.Write("Geçerli bir seçim yapınız (1-4): ");
                }

                switch (choice)
                {
                    case 1:
                        AddProduct();
                        break;
                    case 2:
                        ListProducts();
                        break;
                    case 3:
                        ListOrders();
                        break;
                    case 4:
                        return;
                }
            }
        }

        public void CustomerPanel()
        {
            Console.Clear();
            Console.WriteLine("Müşteri Paneline Hoş Geldiniz!");
            Order order = new Order();
            while (true)
            {
                Console.WriteLine("\n1- Ürünleri Görüntüle");
                Console.WriteLine("2- Sepete Ürün Ekle");
                Console.WriteLine("3- Sipariş Tamamla");
                Console.WriteLine("4- Çıkış");
                Console.Write("Seçiminiz: ");

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || (choice < 1 || choice > 4))
                {
                    Console.Write("Geçerli bir seçim yapınız (1-4): ");
                }

                switch (choice)
                {
                    case 1:
                        ListProducts();
                        break;
                    case 2:
                        AddToCart(order);
                        break;
                    case 3:
                        CompleteOrder(order);
                        return;
                    case 4:
                        return;
                }
            }
        }

        private void AddProduct()
        {
            Console.Write("Ürün Adı: ");
            string name = Console.ReadLine();
            Console.Write("Stok Miktarı: ");
            int stock = int.Parse(Console.ReadLine());
            Console.Write("Fiyat: ");
            decimal price = decimal.Parse(Console.ReadLine());

            products.Add(new Product(name, stock, price));
            Console.WriteLine("Ürün başarıyla eklendi!");
        }

        private void ListProducts()
        {
            Console.WriteLine("\n**************************");
            Console.WriteLine("Ürün Listesi:");
            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine("**************************\n");
        }

        private void AddToCart(Order order)
        {
            ListProducts();
            Console.Write("Sepete eklemek istediğiniz ürün numarasını girin: ");
            int productIndex = int.Parse(Console.ReadLine()) - 1;
            Console.Write("Adet: ");
            int quantity = int.Parse(Console.ReadLine());

            if (productIndex >= 0 && productIndex < products.Count && products[productIndex].Stock >= quantity)
            {
                order.AddItem(products[productIndex], quantity);
                products[productIndex].Stock -= quantity;
                Console.WriteLine("Ürün sepete eklendi.");
            }
            else
            {
                Console.WriteLine("Hatalı giriş veya yetersiz stok!");
            }
        }

        private void CompleteOrder(Order order)
        {
            if (order.Items.Count == 0)
            {
                Console.WriteLine("\nSepetiniz boş!");
                return;
            }

            decimal totalPrice = order.GetTotalPrice();
            orders.Add(order);
            Console.WriteLine("\n**************************");
            Console.WriteLine("Sipariş Tamamlandı!");
            Console.WriteLine($"Toplam Tutar: {totalPrice} TL");
            Console.WriteLine("**************************\n");

            // Konsolun hemen kapanmaması için kullanıcıdan bir tuşa basmasını bekliyoruz.
            Console.WriteLine("Devam etmek için bir tuşa basın...");
            Console.ReadLine();  // Bu satır eklendi
        }

        private void ListOrders()
        {
            Console.WriteLine("\n**************************");
            Console.WriteLine("Siparişler:");
            foreach (var order in orders)
            {
                Console.WriteLine(order);
            }
            Console.WriteLine("**************************\n");
        }
    }

    public class Product
    {
        public string Name { get; }
        public int Stock { get; set; }
        public decimal Price { get; }

        public Product(string name, int stock, decimal price)
        {
            Name = name;
            Stock = stock;
            Price = price;
        }

        public override string ToString()
        {
            return $"{Name} - {Price} TL (Stok: {Stock})";
        }
    }

    public class Order
    {
        public List<OrderItem> Items { get; } = new List<OrderItem>();

        public void AddItem(Product product, int quantity)
        {
            Items.Add(new OrderItem(product, quantity));
        }

        public decimal GetTotalPrice()
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.Product.Price * item.Quantity;
            }
            return total;
        }

        public override string ToString()
        {
            string details = string.Join(", ", Items);
            return $"Sipariş: {details}";
        }
    }

    public class OrderItem
    {
        public Product Product { get; }
        public int Quantity { get; }

        public OrderItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"{Product.Name} x {Quantity}";
        }
    }
}
