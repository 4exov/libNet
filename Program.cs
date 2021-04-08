using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Text;


namespace TestLib
{
    class Book
    {
        public String Name { get; }
        public String Author { get; }
        public String Category { get; }
        public String Language { get; }
        public DateTime PublicDate { get; }
        public String Isbn { get; }
        public Book(string name, string author, string category, string language, DateTime publicDate, string isbn)
        {
            this.Name = name;
            this.Author = author;
            this.Category = category;
            this.Language = language;
            this.PublicDate = publicDate;
            this.Isbn = isbn;
        }       
    }

    class User
    {
        public List<Book> books;

        public String Name { get; }
        public String ID { get; }               
       
        public User(string name, string id)
        {
            this.Name = name;
            this.ID = id;
            this.books = new List<Book>();
           
        }

        public int CountOfBooks()
        {
            return this.books.Count();
        }
        public void TakeBook(Book book){
            this.books.Add(book);
        }  
        public Book RemoveBookByIndx(int index){
            Book book = null;
            if ((index >= 0) && (index < this.books.Count))
            {
                    book = this.books[index];
                    this.books.RemoveAt(index);                    
            }
            // Return null, if index incorrect
            return book;
        }       
    }

    class Library
    {
        private List<Book> books;
        private List<User> users;
        private const string DATABASE_FILE_PATH = "db.json";

        public Library()
        {
            this.books = new List<Book>();
            this.users = new List<User>();
            this.Load();

            // TODO: remove this part.
            // Create test users. Temporary. 
            User user1 = new User("User 1", "111");
            this.AddUser(user1);
            User user2 = new User("User 2", "222");
            this.AddUser(user2);
        }

        private void Load()
        {
            if (File.Exists(DATABASE_FILE_PATH))
            {
                string txt = File.ReadAllText(DATABASE_FILE_PATH);
                this.books = JsonSerializer.Deserialize<List<Book>>(txt);
            }
            else
            {
                Console.WriteLine("File doesn't exist");
            }
        }

        public void Save()
        {
            string json = JsonSerializer.Serialize(this.books);
            File.WriteAllText(DATABASE_FILE_PATH, json);

        }

        public void AddBook(Book book)
        {
            this.books.Add(book);
        }
        private void AddUser(User user)
        {
            this.users.Add(user);
        }

        public string CreateNewBook() {
            Console.WriteLine("Create a new book:");
            Console.WriteLine("  Name of the Book:");
            string bookName = Console.ReadLine();
            Console.WriteLine("  Book\'s author");
            string bookAuthor = Console.ReadLine();
            Console.WriteLine("  Category of the book");
            string category = Console.ReadLine();
            Console.WriteLine("  Language of the Book");
            string bookLang = Console.ReadLine();
            Console.WriteLine("ISBN");
            string isbn = Console.ReadLine();
            
            //Console.WriteLine("NEW BOOK:\n\t Name: {0}\n\t Author = {1}\n\t Category = {2}\n\t Language = {3}\n\t ISBN = {4}\n\t", bookName, bookAuthor, category, bookLang,  isbn);
            // TODO: question -  Correct? Y or N
            
            Book addedBook = new Book(bookName, bookAuthor, category, bookLang, DateTime.Now.Date, isbn);
            this.AddBook(addedBook);
            this.Save();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("NEW BOOK:\n\t Name: {0}\n\t Author = {1}\n\t Category = {2}\n\t Language = {3}\n\t ISBN = {4}\n", bookName, bookAuthor, category, bookLang,  isbn);
            sb.Append("The book was added successfully");
            return sb.ToString();
        }
        
        public string ShowAllBooks(){            
            return Library.ShowAllBooks(this.books);
        }


        public static string ShowAllBooks(List<Book> books)
        {
            if (books.Count > 0 )
            {
                Console.WriteLine("---------LIST OF THE BOOKS--------");
                Console.WriteLine(" # | BOOK NAME | AUTHOR | CATEGORY | LANGUAGE | PUBLISHED | ISBN ");
                for (int i = 0; i < books.Count; i++) {
                    Book book = books[i];
                    
                    Console.WriteLine(" {0} | {1} | {2} | {3} | {4}  {5} | {6} ", 
                        i, book.Name, book.Author, book.Category, book.Language, book.PublicDate.ToString("MM/dd/yyyy"), book.Isbn);
                }     
            }
            else
            {
                Console.WriteLine("Library is empty. Please, add a new book.");
            }
            
            return String.Empty;
        }

        public string ShowAllUsers(){
            if (this.users.Count > 0 )
            {
                Console.WriteLine("---------LIST OF THE USERS--------");
                Console.WriteLine(" # | USER NAME | ID | COUNT BOOKS");
                for (int i = 0; i < this.users.Count; i++) {
                    User user = this.users[i];
                    
                    Console.WriteLine(" {0} | {1} | {2} | {3} ", 
                        i, user.Name, user.ID, user.CountOfBooks());
                }     
            }
            else
            {
                Console.WriteLine("There are no users in the library. Very strange. ");
            }
            
            return String.Empty;
        }


        // Returns empty string if ESC key pressed or result line.
        public static string RadLineOrESC()
        {
            string result = String.Empty;
            StringBuilder buffer = new StringBuilder();

            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter && info.Key != ConsoleKey.Escape)
            {
                Console.Write(info.KeyChar);
                buffer.Append(info.KeyChar);
                info = Console.ReadKey(true);
            } 

            if (info.Key == ConsoleKey.Enter)
            {
                result = buffer.ToString();
            }

            return result;
        }
        
        // Remove book from library. Return result msg
        public string DeleteBook()
        {   
            if (!(this.books.Count > 0))
            {
                return "Library is empty. Please, add a new book.";
            }

            Console.WriteLine(">>> Please, select index of book to delete(press Esc button - back to the Main Menu.)\n");
            this.ShowAllBooks();

            string itemIndex = Library.RadLineOrESC();
            
            if(!String.IsNullOrEmpty(itemIndex))
            {
                int index;
                Book book = null;

                bool success = Int32.TryParse(itemIndex, out index);
                if (success)
                {
                    if ((index >= 0) && (index < this.books.Count))
                    {
                        book = this.books[index];
                        this.books.RemoveAt(index);
                        this.Save();
                        return String.Format("Book \"{0}\" has been succesfully removed", book.Name);
                    }
                    else{
                        return "Index of range";
                        
                    }               
                }
                else
                {
                    return "Only arabic numbers are accepted, like 1 2 3 ...";
                }
            }
            else
            {
                return String.Empty;
            }        
        }

        public string TakeBook()
        {
            Console.WriteLine("Take a book to user. ");
            Console.WriteLine("Please, select book by index(Esc - back to main menu): \n");
            this.ShowAllBooks();
            string  index = Library.RadLineOrESC();
            if(String.IsNullOrEmpty(index))
            {
                return String.Empty;
            }
            int indexOfBook;
            bool success = Int32.TryParse(index, out indexOfBook);
            Book  book = null;
            if ((success) && (indexOfBook >= 0) && (indexOfBook < this.books.Count))
            {
                book = this.books[indexOfBook];
            }
            else 
            {
                return "Incorrect index of book;";
            }
            
            Console.WriteLine("\nPlease, select user by index:(Esc - back to the main menu.\n) ");
            this.ShowAllUsers();
            string  indexUser = Library.RadLineOrESC();
            if(String.IsNullOrEmpty(indexUser))
            {
                return String.Empty;
            }
            int indexOfUser;
            success = Int32.TryParse(indexUser, out indexOfUser);
            User user = null;
            if ( (success) &&(indexOfUser >= 0) && (indexOfUser < this.books.Count))
            {
                user = this.users[indexOfUser];
            }
            else 
            {
                return "Incorrect index of user;";
            }

            if(user.CountOfBooks() < 3)
            {
                 this.books.RemoveAt(indexOfBook);
                user.TakeBook(book);
                return String.Format("User {0} take \"{1}\" book.", user.Name, book.Name);     
            }
            else
            {
                return String.Format("The user {0} has previously take more than 3 books. Please, read and return!" , user.Name);
            }
        }

        public string ReturnBook()
        {
            Console.WriteLine("Remove a book from user. ");            
            Console.WriteLine("\nPlease, select user by index:(Esc - back to the main menu.) ");
            this.ShowAllUsers();
            string  indexUser = Library.RadLineOrESC();
            if(String.IsNullOrEmpty(indexUser))
            {
                return String.Empty;
            }
            
            int indexOfUser;
            bool success = Int32.TryParse(indexUser, out indexOfUser);
            User user = null;
            if ( (success) &&(indexOfUser >= 0) && (indexOfUser < this.books.Count))
            {
                user = this.users[indexOfUser];
            }
            else 
            {
                return "Incorrect index of user;";
            }            
            if(user.CountOfBooks() == 0)
            {
                return String.Format("User {0} has no books. ", user.Name);
            }
            Console.WriteLine("\nPlease, select book by index(Esc - back to main menu): ");
            Library.ShowAllBooks(user.books);
            string  index = Library.RadLineOrESC();
            if(String.IsNullOrEmpty(index))
            {
                return String.Empty;
            }
            int indexOfBook;
            success = Int32.TryParse(index, out indexOfBook);
            Book  book = null;
            if ((success) && (indexOfBook >= 0) && (indexOfBook < this.books.Count))
            {
                book = user.books[indexOfBook];
            }
            else 
            {
                return "Incorrect index of book;";
            }            
            
            user.RemoveBookByIndx(indexOfBook);
            this.books.Add(book);            
            return String.Format("User {0} remove \"{1}\" book.", user.Name, book.Name);
            
        }

        
        public string SearchByFilter()
        {  
            if (this.books.Count == 0)
            {
                return "Library is empty. Please, add a new book.";
            }
            int index = -1;
            bool showSubMenu = true;            
            Console.Clear();
            while (showSubMenu)
            {       
                Console.WriteLine("--------------------------- Search by filter ----------------------------------");
                Console.WriteLine(">> Choose your filter: ");
                Console.WriteLine("     0 - NAME");
                Console.WriteLine("     1 - CATEGORY");
                Console.WriteLine("     2 - AUTHOR");
                Console.WriteLine("     3 - LANGUAGE");
                Console.WriteLine("     4 - ISBN");
                Console.WriteLine("     5 - Back to the main menu");
                Console.WriteLine("-------------------------------------------------------------------------------");
                string input = Console.ReadLine();                
                bool success = Int32.TryParse(input, out index);
                if (success)
                {
                    if((index >= 0) && (index < 5))
                    {
                        showSubMenu = false;
                        
                    }
                    else if (index == 5)
                        {                            
                            return String.Empty;
                        }
                        else
                        {                            
                            Console.WriteLine("Incorrect input. Try again.\n");
                        }
                }
            }
            
            Console.WriteLine(">> Please, input your request (Esc - back to the main menu): ");
            string searchReq = Library.RadLineOrESC();
            if(String.IsNullOrEmpty(searchReq))
            {
                return String.Empty;
            }
            
            
            //Search by filter.
            List<Book> resultList = null;
            searchReq = searchReq.ToLower();
            switch(index){
                case 0:
                    resultList = this.books.Where(item => item.Name.ToLower().Contains(searchReq)).ToList();            
                    break;
                case 1:
                    resultList = this.books.Where(item => item.Category.ToLower().Contains(searchReq)).ToList();
                    break;
                case 2:
                    resultList = this.books.Where(item => item.Author.ToLower().Contains(searchReq)).ToList();               
                    break;
                case 3:
                    resultList = this.books.Where(item => item.Language.ToLower().Contains(searchReq)).ToList();
                    break;
                case 4:                           
                    resultList = this.books.Where(item => item.Isbn.ToLower().Contains(searchReq)).ToList();
                    break;
                default:
                    return "Something went wrong!";

            }
                        
            if(resultList.Count > 0)
            {
                Console.WriteLine("\nRESULT SEARCHING:\n");                
                Library.ShowAllBooks(resultList);
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                return String.Empty;
            }           
            else
            {
                return String.Format( "Search result by this filter \"{0}\", return nothing :(", searchReq)  ;
            }            
        }
    }

    class Program
    {
        public static void ShowMenu(string firstMsg)
        {
            
            Console.Clear();
            if (! String.IsNullOrEmpty(firstMsg)){
                Console.WriteLine("\n{0}\n", firstMsg);
       
            }
            Console.WriteLine("--------------------------- Menu ----------------------------------");
            Console.WriteLine(">> Choose your action: ");
            Console.WriteLine("     1 - add book");
            Console.WriteLine("     2 - to delete book");
            Console.WriteLine("     3 - take book");
            Console.WriteLine("     4 - return book");
            Console.WriteLine("     5 - show list of all books");
            Console.WriteLine("     6 - arrange list of books by: ");
            Console.WriteLine("     7 - exit");
            Console.WriteLine("--------------------------------------------------------------------");
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the \"Console Library\"");
            Library lib = new Library();
                        
            string result = String.Empty;
            bool runMenu = true;
            while (runMenu)
            {
                Program.ShowMenu(result);

                string choice = Console.ReadLine();                
                switch(choice){
                    case "1":
                        result = lib.CreateNewBook();
                        break;
                    case "2":
                        result = lib.DeleteBook();                       
                        break;
                    case "3":
                        result = lib.TakeBook();
                        break;
                    case "4":                           
                        result = lib.ReturnBook();
                        break;
                    case "5":
                        result = lib.ShowAllBooks();
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey(true);
                        break;
                    case "6":
                        result = lib.SearchByFilter();
                        // Console.WriteLine("Press any key to continue");
                        // Console.ReadKey(true);                        
                        break;

                    case "7":
                        Console.WriteLine("Thank you for visiting our library today", choice);
                        runMenu = false;
                        break;
                    default:
                        result = "Choose your number between 1 and 7";
                        break;
                    
                    }
                }
            {

            }
        }
    }
}


