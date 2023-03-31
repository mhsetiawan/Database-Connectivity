using System.Data.SqlClient;
using System.Diagnostics;

namespace DbConnectivity;

class program 
{

    static string ConnectionString = "Data Source=HARIS;Initial Catalog=db_hr_dts;Integrated Security=True;Connect Timeout=30";

    static SqlConnection connection;
    static void Main(string[]args)
    {

        GetRegion();
        //GetRegionById(2);
        //InsertRegion("Antartika");
        //DeleteRegionById(6);
        //UpdateRegion("Europe", 1);
    }

    public static void GetRegion()
    {
        //mendefinisikan koneksi dengan data source/database yang telah ditentukan,
        connection = new SqlConnection(ConnectionString);

        //membuat command sql
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "Select * from tbl_region";

        //membuka koneksi
        connection.Open();

        //jalankan command sql
        using SqlDataReader reader = command.ExecuteReader();
        //mengecek hasil command sql apakah ada record data
        if (reader.HasRows)
        {
            //mengecek record data per baris dan menampilkan record data hasil sql
            while (reader.Read())
            {
                Console.WriteLine("Id   : " + reader[0]);
                Console.WriteLine("Name   : " + reader[1]);
                Console.WriteLine("========================");
            }
        }
        else {
            //Jika hasil command sql tidak ada record data yang ditemukan
            Console.WriteLine("Data Not Found!");
        }
        //menutup sql reader dan connection
        reader.Close();
        connection.Close();

    }
    public static void GetRegionById(int id) {
        //mendefinisikan koneksi dengan data source/database yang telah ditentukan,
        connection = new SqlConnection(ConnectionString);

        //membuka koneksi
        connection.Open();

        //membuat command sql yang dinamis dengan parameter
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "Select * From tbl_region Where Id=@id";

        //Membuat parameter untuk digunakan pada command sql
        SqlParameter pId = new SqlParameter();
        pId.ParameterName = "@id";
        pId.Value = id;
        pId.SqlDbType = System.Data.SqlDbType.Int;
        //menambahkan parameter ke command sql
        command.Parameters.Add(pId);

        //jalankan command sql
        using SqlDataReader reader = command.ExecuteReader();
        //mengecek hasil command sql apakah ada row/baris record
        if (reader.HasRows)
        {
            //mengecek record data per baris dan menampilkan record data hasil sql
            while (reader.Read())
            {
                Console.WriteLine("Id   : " + reader[0]);
                Console.WriteLine("Name   : " + reader[1]);
                Console.WriteLine("========================");
            }
        }
        else
        {
            //Jika hasil command sql tidak ada record data yang ditemukan
            Console.WriteLine("Data Not Found!");
        }
        //menutup sql reader dan connection
        reader.Close();
        connection.Close();
    }

    public static void InsertRegion(string name) 
    {
        //mendefinisikan koneksi dengan data source/database yang telah ditentukan
        connection = new SqlConnection(ConnectionString);

        //membuka koneksi
        connection.Open();
        
        //mendefinisikan transaction yang dapat terdiri lebih dari 1 perintah (Commit/Rollback)
        SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            //membuat command sql yagn dinamis dengan parameter
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "Insert into tbl_region (name) Values (@name)";
            command.Transaction = transaction;

            //Membuat parameter untuk digunakan pada command sql
            SqlParameter pName = new SqlParameter();
            pName.ParameterName = "@name";
            pName.Value = name;
            pName.SqlDbType = System.Data.SqlDbType.VarChar;

            //menambahkan parameter ke command
            command.Parameters.Add(pName);

            //jalankan command sql
            int result = command.ExecuteNonQuery();
            //mengizinkan perubahan sesuai command sql yang dieksekusi
            transaction.Commit();

            //mengecek command sql apakah ada row/baris pada tabel hasi command sql
            if (result > 0)
            {
                //Jika berhasil menambahkan record 
                Console.WriteLine("Data Berhasil Ditambahkan");
            }
            else
            {
                //Jika tidak menambahkan record
                Console.WriteLine("Data Gagal Ditambahkan");
            }
            //menutup koneksi
            connection.Close();
        }
        catch (Exception e){
            Console.WriteLine(e.Message);
            try
            {
                //membatalkan perubahan yang dilakukan oleh command transaksi
                transaction.Rollback();
            }
            catch (Exception rollback) {
                //menampilkan pesan eror jika rollback gagal atau ada exception lainnya
                Console.WriteLine(rollback.Message);
            }
        }
    }

    public static void DeleteRegionById(int id) {
        //mendefinisikan koneksi dengan data source/database yang telah ditentukan
        connection = new SqlConnection(ConnectionString);

        //membuka koneksi
        connection.Open();
        //mendefinisikan transaction yang dapat terdiri lebih dari 1 perintah (Commit/Rollback)
        SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            //membuat command sql yanf dinamis dengan parameter
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "Delete From tbl_region Where Id=@id";
            command.Transaction = transaction;

            //Membuat parameter untuk digunakan pada command sql
            SqlParameter pId = new SqlParameter();
            pId.ParameterName = "@id";
            pId.Value = id;
            pId.SqlDbType = System.Data.SqlDbType.Int;

            //menambahkan parameter ke command
            command.Parameters.Add(pId);

            //menjalankan command
            int result = command.ExecuteNonQuery();
            //mengizinkan perubahan sesuai command sql yang dieksekusi
            transaction.Commit( );

            //mengecek command sql apakah ada row/baris pada tabel hasi command sql
            if (result > 0)
            {
                //Jika berhasil menghapus record
                Console.WriteLine("Data Berhasil Dihapus");
            }
            else
            {
                //Jika tidak berhasil menghapus record
                Console.WriteLine("Data Gagal Dihapus");
            }
            //menutup koneksi
            connection.Close();
        }
        catch (Exception e){
            Console.WriteLine(e.Message);
            try
            {
                //membatalkan perubahan yang dilakukan oleh command transaksi
                transaction.Rollback();
            }
            catch(Exception rollback){
                //menampilkan pesan eror jika rollback gagal atau ada exception lainnya
                Console.WriteLine(rollback.Message);
            }
        }
    }

    public static void UpdateRegion(string update, int id) {
        //mendefinisikan koneksi dengan data source/database yang telah ditentukan
        connection = new SqlConnection(ConnectionString);
        //membuka koneksi
        connection.Open();
        //mendefinisikan transaction yang dapat terdiri lebih dari 1 perintah (Commit/Rollback)
        SqlTransaction transaction = connection.BeginTransaction();
        try {
            //membuat command sql yang dinamis
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "Update tbl_region set name=@update Where Id=@id";
            command.Transaction = transaction;

            //Membuat parameter untuk digunakan pada command sql
            SqlParameter pUpdate = new SqlParameter();
            pUpdate.ParameterName = "@update";
            pUpdate.Value = update;
            pUpdate.SqlDbType = System.Data.SqlDbType.VarChar;

            SqlParameter pId = new SqlParameter();
            pId.ParameterName = "@id";
            pId.Value = id;
            pId.SqlDbType = System.Data.SqlDbType.Int;

            //menambahkan parameter ke command
            command.Parameters.Add(pUpdate);
            command.Parameters.Add(pId);

            //menjalankan command
            int result = command.ExecuteNonQuery();
            //mengizinkan perubahan sesuai command sql yang dieksekusi
            transaction.Commit();

            //mengecek command sql apakah ada row/baris pada tabel hasi command sql
            if (result > 0) {
                //jika berhasil update
                Console.WriteLine("Update Berhasil");
            }
            else {
                //jika gagal update
                Console.WriteLine("Update Gagal");
            }
            //menutup koneksi
            connection.Close();
        }catch(Exception e) {
            Console.WriteLine(e.Message);
            try
            {
                //membatalkan perubahan yang dilakukan oleh command transaksi
                transaction.Rollback();
            }
            catch (Exception rollback) {
                //menampilkan pesan eror jika rollback gagal atau ada exception lainnya
                Console.WriteLine(rollback.Message);
            }
        }
        
        

    }

}
