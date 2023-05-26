using BlogApp.DAL;
using BlogApp.Dto;
using BlogApp.Models;

namespace BlogApp.Services;

public class UserService
{
    private readonly BlogContext _context;

    public UserService(BlogContext context)
    {
        _context = context;
    }

    public User Register(RegisterDto registerDto)
    {
        var user = new User
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            HashPassword = registerDto.Password
        };
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }
    public User? FindByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }
    public User? Login(LoginDto loginDto)
    {
        var user = FindByEmail(loginDto.Email);
        if (user == null)
        {
            return null;
        }
        return user.HashPassword != loginDto.Password ? null : user;
    }
}