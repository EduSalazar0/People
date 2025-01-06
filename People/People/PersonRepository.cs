using SQLite;
using People.Models;
using System.Threading.Tasks;


namespace People;

public class PersonRepository
{
    private SQLiteAsyncConnection conn;
    string _dbPath;

    public string StatusMessage { get; set; }

    // TODO: Add variable for the SQLite connection

    private async Task Init()
    {
        if(conn != null)
            return;

        conn = new SQLiteAsyncConnection(_dbPath);
        
          
    }

    public PersonRepository(string dbPath)
    {
        _dbPath = dbPath;                        
    }

    public async Task AddNewPerson(string name)
    {            
        int result = 0;
        try
        {
            await Init();

            
            if (string.IsNullOrEmpty(name))
                throw new Exception("Valid name required");

            
            result = await conn.InsertAsync(new Person { Name = name });

            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
        }

    }

    public async Task<List<Person>> GetAllPeople()
    {
        
        try
        {
            await Init();
            return await conn.Table<Person>().ToListAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Person>();
    }

    public async Task DeleteAsync(string name)
    {
        try
        {
            await Init();

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Se requiere un nombre válido.");
            }

            // Buscar la persona por nombre.
            var person = await conn.Table<Person>().FirstOrDefaultAsync(a => a.Name == name);


            if (person == null)
            {
                throw new Exception($"No se encontró una persona con el nombre '{name}'.");
            }

            // Eliminar el objeto encontrado.
            await  conn.DeleteAsync(person);
            

            StatusMessage = $"Registro eliminado exitosamente (Nombre: {name}).";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error al eliminar {name}. Detalles: {ex.Message}";
        }


    }
    
}
