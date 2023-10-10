class Weapon
{
    private int _damage;
    private int _bullets;

    public Weapon(int damage, int bullets)
    {
        if (bullets < 0)
            throw new ArgumentOutOfRangeException(nameof(bullets));

        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        _damage = damage;
        _bullets = bullets;
    }

    public bool HasBullets => _bullets > 0;

    public void Fire(Player player)
    {
        if (HasBullets)
        {
            player.TakeDamage(_damage);
            _bullets--;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}

class Player
{
    private int _health;

    public Player(int health)
    {
        if (health < 0)
            throw new ArgumentOutOfRangeException(nameof(health));

        _health = health;
    }

    public bool IsAlive => _health > 0;

    public void TakeDamage(int damage)
    {
        if (IsAlive)
            _health -= damage;
    }
}

class Bot
{
    private Weapon _weapon;

    public Bot(Weapon weapon)
    {
        if (weapon == null)
            throw new ArgumentNullException(nameof(weapon));

        _weapon = weapon;
    }

    public void OnSeePlayer(Player player)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player));

        _weapon.Fire(player);
    }
}
