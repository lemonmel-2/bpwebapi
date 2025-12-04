using System.Text.Json;
using webapi.Enum;
using webapi.Exception;
using webapi.Model;
using webapi.Repository;

public class UserRepo : IUserRepo
{
    
    // singleton pattern to ensure only one instance of user repo
    private static readonly UserRepo instance = new UserRepo();

    public static UserRepo Instance => instance;

    private readonly List<User> userList;

    private readonly string filePath = "../consoleApp/repository/tempStorage/userDb.json";

    private UserRepo()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            userList = JsonSerializer.Deserialize<List<User>>(json)?? new List<User>();
        }
        else
        {
            userList = new List<User>();
            SaveChanges();
        }
    }

    public void AddUser(User user)
    {
        userList.Add(user);
        SaveChanges();
    }

    public User[] GetTopUsers()
    {
        User[] users = userList.OrderByDescending(user => user.HighestScore).ToArray();
        return users;
    }

    public User GetUser(string userId)
    {
        try
        {
            User user = userList.FirstOrDefault(user => user.UserId == userId);
            return user;
        }
        catch(ArgumentNullException)
        {
            throw new GameException(ErrorCode.USER_NOT_EXIST);
        }
    }

    public void UpdateUser(User user)
    {
        try
        {
            User selectUser = userList.FirstOrDefault(u => u.UserId == user.UserId);
            selectUser.HighestScore = user.HighestScore;
            selectUser.Items = user.Items;
            SaveChanges();
        }
        catch (ArgumentNullException)
        {
            throw new GameException(ErrorCode.USER_NOT_EXIST);
        }
    }

    //Update Json File
    private void SaveChanges()
    {
        try
        {
            string json = JsonSerializer.Serialize(userList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        catch (IOException)
        {
            throw new GameException(ErrorCode.DATABASE_ERROR);
        }
    }
}