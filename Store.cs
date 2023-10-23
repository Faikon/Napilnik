using System;
using System.Collections.Generic;

namespace Store
{
    internal class Store
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            //Вывод всех товаров на складе с их остатком
            warehouse.Show();

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            //Вывод всех товаров в корзине
            cart.Show();

            Console.WriteLine(cart.Order().Paylink);

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
        }
    }
    public class Shop
    {
        private Warehouse _warehouse;
        private Cart _cart;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse ?? throw new ArgumentNullException(nameof(warehouse));
        }

        public string Paylink { get; private set; } = "Paylink";

        public Cart Cart()
        {
            return new Cart(this);
        }

        public bool TrySell(Good good, int amount)
        {
            if (_warehouse.IsContain(good, amount))
            {
                _warehouse.Remove(good, amount);
                return true;
            }
            else
            {
                Console.WriteLine("Not enough goods in warehouse");
                return false;
            }
        }
    }

    public class Good
    {
        public Good(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; private set; }
    }

    public class Warehouse
    {
        protected Dictionary<Good, int> _goods = new Dictionary<Good, int>();

        public void Delive(Good good, int amount)
        {
            if (_goods.ContainsKey(good))
                _goods[good] += amount;
            else
                _goods.Add(good, amount);
        }

        public void Remove(Good good, int amount)
        {
            if (_goods.ContainsKey(good))
            {
                if (_goods[good] - amount == 0)
                    _goods.Remove(good);
                else
                    _goods[good] -= amount;
            }
        }

        public void Show()
        {
            foreach (var good in _goods)
                Console.WriteLine($"{good.Key.Name} - {good.Value}");
        }

        public bool IsContain(Good good, int amount)
        {
            return _goods.ContainsKey(good) && (_goods[good] - amount >= 0);
        }
    }

    public class Cart : Warehouse
    {
        private Shop _shop;

        public Cart(Shop shop)
        {
            _shop = shop ?? throw new ArgumentNullException(nameof(shop));
        }

        public void Add(Good good, int amount)
        {
            if (_shop.TrySell(good, amount))
                Delive(good, amount);
        }

        public Shop Order()
        {
            return _shop;
        }
    }
}
