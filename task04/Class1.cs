namespace task04
{
   public interface ISpaceship
 {
    void MoveForward();      // Движение вперед
    void Rotate(int angle);  // Поворот на угол (градусы)
    void Fire();             // Выстрел ракетой
    int Speed { get; }       // Скорость корабля
    int FirePower { get; }   // Мощность выстрела
 }

 // Класс крейсера
 public class Cruiser : ISpaceship
 {
    public int Speed { get; private set; } = 50;     // Медленный
    public int FirePower { get; private set; } = 100; // Мощные ракеты
    
    public void MoveForward()
    {
        Console.WriteLine($"Крейсер движется вперед со скоростью {Speed} единиц");
    }
    
    public void Rotate(int angle)
    {
        Console.WriteLine($"Крейсер поворачивается на {angle} градусов (медленный разворот)");
    }
    
    public void Fire()
    {
        Console.WriteLine($"Крейсер стреляет ракетой! Урон: {FirePower}");
    }
 }

 // Класс истребителя
 public class Fighter : ISpaceship
 {
    public int Speed { get; private set; } = 150;    // Быстрый
    public int FirePower { get; private set; } = 30; // Слабые ракеты
    
    public void MoveForward()
    {
        Console.WriteLine($"Истребитель летит со скоростью {Speed} единиц");
    }
    
    public void Rotate(int angle)
    {
        Console.WriteLine($"Истребитель быстро разворачивается на {angle} градусов");
    }
    
    public void Fire()
    {
        Console.WriteLine($"Истребитель выпускает ракеты! Урон каждой: {FirePower}");
    }
 }
}