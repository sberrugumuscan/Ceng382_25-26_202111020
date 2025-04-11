using Microsoft.AspNetCore.Mvc; //ASP.NET Core içindeki controller ve action mekanizması kullanımı için
using Microsoft.AspNetCore.Mvc.RazorPages; //Razor Pages API kullanabilmek için
using IndexPage.Models;//ClassInformationmodels eişim
using System.Collections.Generic;//koleksiyon sınıflarını kullanka için -List<T>-
using System.Linq;//Listelerde arama sıralama işlemleri için-orderby max firsordefault-

namespace IndexPage.Pages  //IndexPage alanında Pages isimli alt alan tanımlanıyp
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

        public List<ClassInformationTable> FilteredPagedClasses { get; set; } = new();//filtrelenmiş ce sayfalama yapılmış snıf biligerlini tutan liste
        public string? ClassNameFilter { get; set; }//sınıf adı için uyguladığı filtreyi tutacak
        public int? StudentCountFilter { get; set; }//öğrenci sayısı için uygulanan filtre
        public int CurrentPage { get; set; }//görüntülenen sayfa numarasını tutacak
        public int TotalPages { get; set; }//toplam sayfa sayısını tutuacak

        private void EnsureDataIsLoaded() // classes listesi boşsa 100 hazır lsite
        {
            if (!Classes.Any())
            {
                
                for (int i = 1; i <= 100; i++)
                {
                    Classes.Add(new ClassInformationModel
                    {
                        Id = i,
                        ClassName = $"Class {i}",
                        StudentCount = 10 + (i % 50),  //  sayı 10 ile 60 arasında değişiyor
                        Description = $"Description for Class {i}"
                    });
                }
            }
        }

        //sayfa ilk kez yüklendiğinde veya yapılan değişikler sonrasında tetiklencek
        public IActionResult OnGet(string classNameFilter, int? studentCountFilter, int pageNumber = 1)
        {
            EnsureDataIsLoaded();  // sınıf yoksa yapay sınıflar gelsin
    
            //kullanıcı tarafından girilen filtre değerlerini alıp modeldeki filtreme özelliklere atar
            ClassNameFilter = classNameFilter;
            StudentCountFilter = studentCountFilter;

            var filteredClasses = Classes.AsQueryable();//classes listesinde sorgu yapabilmek için IQueryable türüne dönüştü
            //lınq sorguları kullanarak filtreme yapmayı sağlasın

            //kullanıcı sınıf adı girdiyse oan göre öğreci sayısı girdiyse ona göre filtreme yapsın
            if (!string.IsNullOrEmpty(classNameFilter))
                filteredClasses = filteredClasses.Where(c => c.ClassName.Contains(classNameFilter, StringComparison.OrdinalIgnoreCase));

            if (studentCountFilter.HasValue)
                filteredClasses = filteredClasses.Where(c => c.StudentCount == studentCountFilter.Value);

            int pageSize = 10;//her sayfada 10 öge göster
            var totalRecords = filteredClasses.Count();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            CurrentPage = pageNumber < 1 ? 1 : (pageNumber > TotalPages ? TotalPages : pageNumber);
            //kullanıcı tarafında gönderilen sayfa numarasını kontol eder; <1 ise 1 olarak ayalrlanır yadeyse >top.sayfa ise son sayfa olarak

            var pagedClasses = filteredClasses
                .Skip((CurrentPage - 1) * pageSize) //skip önceki sayfları atlicak skip(10): ilk 10 ögeyi atla 11ç.ögeden başla
                .Take(pageSize)                     //take belirtilen sayıda öge alcak  take(10):sadece 10 öge al
                .Select(c => new ClassInformationTable //classınformaitontable mmodeline dönüştürülüyo
                {
                    ClassName = c.ClassName,
                    StudentCount = c.StudentCount,
                    Description = c.Description,
                    Id = c.Id
                })
                .ToList();//veriyi listeye dönüştür

            FilteredPagedClasses = pagedClasses; //filtrelenen ve sayfalanan sınıf lstesi filteredpagedclasses lsitesine atancak

            return Page();
        }



        public IActionResult OnPostAdd()
        {   
            //NewClass nesnesi boşsa,sınıf adı boşsa,öğrenci sayısı<=0 sa hata versin
            if (NewClass == null || string.IsNullOrWhiteSpace(NewClass.ClassName) || NewClass.StudentCount <= 0)
            {   
                ModelState.AddModelError(string.Empty, "Invalid class information.");
                return Page();//sayfa yeniden yüklenir formdakiler kaybolamz
            }

            var existingClass = Classes.FirstOrDefault(c => c.Id == NewClass.Id);
            //daha önce eklenen sınıfı bulur
            if (existingClass != null) //aynı id ise güncelleme 
            {
                Classes.Remove(existingClass);// eski kaydı silecek
            }
            else//id boşsa bu yeni ekleme işlemi id oluştur
            {
                NewClass.Id = Classes.Count > 0 ? Classes.Max(c => c.Id) + 1 : 1;
                //Classes.Count > 0 ? kontrol et,listede sınıflar varsa en yüksek id al ve +1 yap
                //boşsa ilk eklenene 1 ver
            }

            Classes.Add(NewClass); //yeni sınıfı lsiteye ekle
            
            //  listeyi düzgün sırala -> OrderBy(c => c.Id)
            //.ToList ile sıralana yeni listeye aktarılıyor
            var sortedClasses = Classes.OrderBy(c => c.Id).ToList(); //ıd lere göre sırala
            for (int i = 0; i < sortedClasses.Count; i++)  
            {
                if (existingClass == null) // sadece yeni eklenenler için ID güncelle
                {
                    sortedClasses[i].Id = i + 1;
                }
            }
            Classes = sortedClasses;// güncellenmiş listeyi kaydet

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
                    Classes[i].Id = i + 1;    //tekrar 1 den başlayarak srıala
                }
            }
            return RedirectToPage(); // Sayfayı yenileyerek güncel listeyi göster
        }

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