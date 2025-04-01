using Microsoft.AspNetCore.Mvc; //ASP.NET Core içindeki controller ve action mekanizması kullanımı için
using Microsoft.AspNetCore.Mvc.RazorPages; //Razor Pages API kullanabilmek için
using IndexPage.Models;//ClassInformationmodels eişim
using System.Collections.Generic;//koleksiyon sınıflarını kullanka için -List<T>-
using System.Linq;//Listelerde arama sıralama işlemleri için-orderby max firsordefault-

namespace IndexPage.Pages //IndexPage alanında Pages isimli alt alan tanımlanıyp
{
    public class IndexModel : PageModel //ındexmodel , pagemodel sınfıdıadn türerildi.razor pages in post-get işlemlerini yürütebilmek için
    {
        public static List<ClassInformationModel> Classes = new List<ClassInformationModel>();
        //static-->tüm sınıflar için ortak,sayfa çağrıldıkça kaybolmicak
        //sayfaya eklenen sınıfları tutcak
        //new List<ClassInformationModel>(); -> classes lsitesi başlangıçta boş liste

        [BindProperty]
        public ClassInformationModel? NewClass { get; set; }
        //formdan gelen veriler buraya otomatik baplancak,ASP.NET Core otomatik dolduruyo-->bindproperty
        //?,nullable(boş olabilir)

        public void OnGet()//sayfa ilk açıldığında çağrılan metot
        {  
            
        }

        
        public IActionResult OnPostAdd() //formdan ver igönderilidğinde ,IactionResult dönmesi sayfanın yönlendirilmeisni ve hata mesajlarının gösterilmesini sağşar
        {
            //NewClass nesnesi boşsa,sınıf adı boşsa,öğrenci sayısı<=0 sa hata versin
            if (NewClass == null || string.IsNullOrWhiteSpace(NewClass.ClassName) || NewClass.StudentCount <= 0)
            {   
                ModelState.AddModelError(string.Empty, "Invalid class information.");
                return Page();//sayfa yeniden yüklenir formdakiler kaybolamz
            }

            var existingClass = Classes.FirstOrDefault(c => c.Id == NewClass.Id);
            //daha önce eklenen sınıfı bulur
            if (existingClass != null)//aynı id ise güncelleme 
            {
                Classes.Remove(existingClass); // eski kaydı silecek
            }
            else//id boşsa bu yeni ekleme işlemi id oluştur
            {
                NewClass.Id = Classes.Count > 0 ? Classes.Max(c => c.Id) + 1 : 1;
                //Classes.Count > 0 ? kontrol et,listede sınıflar varsa en yüksek id al ve +1 yap
                //boşsa ilk eklenene 1 ver
            }

            
            Classes.Add(NewClass);// yeni eklenen sınıfı listeye ekle

            //  listeyi düzgün sırala -> OrderBy(c => c.Id)
            //.ToList ile sıralana yeni listeye aktarılıyor
            var sortedClasses = Classes.OrderBy(c => c.Id).ToList();
            for (int i = 0; i < sortedClasses.Count; i++)
            {
                if (existingClass == null) // sadece yeni eklenenler için ID güncelle
                {
                    sortedClasses[i].Id = i + 1;
                }
            }
            Classes = sortedClasses; // güncellenmiş listeyi kaydet

            
            NewClass = new ClassInformationModel();//formu sıfırlamak için NewClass yeni bir nesne olarak atanır

            return RedirectToPage();//yeniden yükle sayfayı güncellenmiş sayfayı görelim
        }

        public IActionResult OnPostDelete(int id)
        {    
            //FirstOrDefault() ile id si eşlenen sınıfı bul
            //sınıf yoksa classToDelete null olcak
            var classToDelete = Classes.FirstOrDefault(c => c.Id == id);
            if (classToDelete != null) 
            {
                Classes.Remove(classToDelete);//sınıf nesnesini sil
                for (int i = 0; i < Classes.Count; i++)//kalanların id sini düzenle
                {
                    Classes[i].Id = i + 1;             //tekrar 1 den başlayarak srıala

                }
            }
            return RedirectToPage(); // Sayfayı yenileyerek güncel listeyi göster
        }

        // Sınıf düzenleme işlemi
        public IActionResult OnPostEdit(int id)
        {
            var classToEdit = Classes.FirstOrDefault(c => c.Id == id);
            //sınıf bulunmazsa classToEdit null olcak
            if (classToEdit != null)
            {
                //bulunan sınıf bilgileri NewClass nesnesine kopyaladnı
                //mevcut verileri gör ki düzeltebil
                NewClass = new ClassInformationModel
                {
                    Id = classToEdit.Id,
                    ClassName = classToEdit.ClassName,
                    StudentCount = classToEdit.StudentCount,
                    Description = classToEdit.Description
                };
            }
            return Page(); // sayfayı tekrar yükler
        }
    }
}
