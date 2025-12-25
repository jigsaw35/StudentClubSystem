using System.Linq.Expressions;

namespace StudentClubSystem.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        // Temel CRUD
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);

        // GELİŞMİŞ LİSTELEME
        // filter: Filtreleme (Where)
        // orderBy: Sıralama
        // includeProperties: İlişkili tablolar (Join) - Örn: "Club,User"
        List<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = ""
        );

        // Tekil kayıt getirirken de include gerekebilir (Örn: Etkinlik Detay)
        T Get(
            Expression<Func<T, bool>> filter,
            string includeProperties = ""
        );
    }
}