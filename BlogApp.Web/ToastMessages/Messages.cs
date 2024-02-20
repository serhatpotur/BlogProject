using BlogApp.Entities.DTOs.Roles;

namespace BlogApp.Web.ToastMessages
{
    public static class Messages
    {
        public static class Article
        {
            public static string Add(string articleTitle)
            {
                return $"{articleTitle} başlıklı makaleniz başarılı bir şekilde eklenmiştir";
            }
            public static string Update(string articleTitle)
            {
                return $"{articleTitle} başlıklı makaleniz başarılı bir şekilde güncellenmiştir";
            }
            public static string Delete(string articleTitle)
            {
                return $"{articleTitle} başlıklı makaleniz başarılı bir şekilde silinmiştir";
            }
            public static string UndoDelete(string articleTitle)
            {
                return $"{articleTitle} başlıklı makaleniz başarılı bir şekilde geri alınmıştır";
            }
        }
        public static class Category
        {
            public static string Add(string categoryName)
            {
                return $"{categoryName} isimli kategori başarılı bir şekilde eklenmiştir";
            }
            public static string Update(string categoryName)
            {
                return $"{categoryName} isimli kategori başarılı bir şekilde güncellenmiştir";
            }
            public static string Delete(string categoryName)
            {
                return $"{categoryName} isimli kategori başarılı bir şekilde silinmiştir";
            }
            public static string UndoDelete(string categoryName)
            {
                return $"{categoryName} başlıklı kategori başarılı bir şekilde geri alınmıştır";
            }
        }

        public static class Role
        {
            public static string Add(string username, List<string> roles)
            {
                return $"{username} adlı kullanıcıya {roles} isimli roller başarılı bir şekilde eklenmiştir";
            }
            public static string Update(string username, string roles)
            {
                return $"{username} adlı kullanıcının rolleri {roles} olarak değiştirilmiştir";
            }
            public static string Delete(string categoryName)
            {
                return $"{categoryName} isimli kategori başarılı bir şekilde silinmiştir";
            }
        }
        public static class User
        {
            public static string Add(string username, List<string> roles)
            {
                return $"{username} adlı kullanıcıya {roles} isimli roller başarılı bir şekilde eklenmiştir";
            }
            public static string Update(string username, string roles)
            {
                return $"{username} adlı kullanıcının rolleri {roles} olarak değiştirilmiştir";
            }
            public static string Delete(string username)
            {
                return $"{username} isimli kullanıcı başarılı bir şekilde silinmiştir";
            }
            public static string CurrentPassword()
            {
                return $"Mevcut şifrenizi yanlış girdiniz";
            }
            public static string NewPasswordNotEqualConfirmPassword()
            {
                return $"Girilen şifreler aynı değil ";
            }
            public static string PasswordChangeSuccessfuly()
            {
                return $"Şifreniz başarılı bir şekilde değiştirilmiştir";
            }
        }
    }
}
