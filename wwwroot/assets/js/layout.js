$(function () {

    FixedHeader();
    LazyLoadingImage();
    SelectLangChange();

    SmartPhoto();

    //Khoi tao recent view
    $("body").recentView();

    WidgetGalleryImageHandler.init();
})

;(function () {
    var o = $({});
    $.each({
        trigger: 'publish',
        on: 'subscribe',
        off: 'unsubscribe'
    }, function (key, val) {
        $[val] = function () {
            o[key].apply(o, arguments);
        }
    });
})();

function FixedHeader() {
    const header_container = $(".header_container");
    const header_containerPosition = header_container?.offset()?.top;

    $(window).on("scroll", (e) => {
        let windowPos = $(window).scrollTop();

        if (windowPos > header_containerPosition) {
            header_container.addClass("stuck");
        } else {
            header_container.removeClass("stuck");
        }
    })
};

function Enter(callback) {
    return (e) => {
        if (e.key === "Enter") {
            e.preventDefault();

            callback(e)
        }
    }
};

function LazyLoadingImage() {
    let getAllImageElement = document.querySelectorAll('img[loading]');

    let observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
            if (entry.isIntersecting) {
                LoadImage(entry.target);
            }
        })
    })

    getAllImageElement.forEach((img) => {
        //check isLoaded
        let isLoaded = img.classList.contains("loaded");
        if (!isLoaded) {
            img.classList.add("fadeOutImage");
        }

        observer.observe(img);
    })

    function LoadImage(img) {
        //check isLoaded
        let isLoaded = img.classList.contains("loaded");
        if (!isLoaded) {
            let imgSrc = img.getAttribute('data-src');
            img.classList.remove("isLoading");

            img.setAttribute('src', imgSrc);
            img.classList.remove("fadeOutImage");

            img.className = img.className + " fadeInImage loaded";

            img.removeAttribute('data-src');
        }
    }
};

function SelectLangChange() {
    $("#select_language, #select_language_mobile").change((e) => {
        let target = $(e.target);
        let languageURL = $(target).find("option:selected").attr("currenturl");

        window.location.href = languageURL;
    })
};

;(function () {
    $.fn.recentView = function (options) {
        let settings = $.extend({}, options)

        let viewedProductStorage = {
            add: function (item) {

            },
            remove: function (item) {

            },
            getAll: function () {
                return JSON.parse(localStorage.getItem('viewedProducts')) || [];
            },
            exist: function (item) {
                let items = this.getAll()
                return items.includes(item)
            },
            save: function (items) {
                localStorage.setItem("viewedProducts", JSON.stringify(items));
            }
        }


        function cache($this) {
            settings.$container = $this
            settings.$recentview_productList = $this.find(".recentview_productList")
        }

        function events() {
            //On first load()
            if (settings.$recentview_productList?.length > 1) {
                Load()
            }

            function Load() {
                let productDetailContainer = $(".product_detail_container")?.eq(0)
                if (settings.$container.length > 0) {
                    //Hien thi items
                    if (viewedProductStorage.getAll().length > 0) {
                        let Ids = viewedProductStorage.getAll()
                        API(Ids).then((items) => {
                            if (items) {
                                RenderViewedProducts(items)

                                //Khoi tao slick
                                settings.$recentview_productList.slick({
                                    cellAlign: 'left',
                                    prevNextButtons: true,
                                    freeScroll: true,
                                    pageDots: false,
                                    slidesToShow: 4,
                                    slidesToScroll: 1,
                                    responsive: [
                                        {
                                            breakpoint: 900,
                                            settings: {
                                                rows: 1,
                                                slidesToShow: 3,
                                                slidesToScroll: 1
                                            }
                                        },
                                        {
                                            breakpoint: 600,
                                            settings: {
                                                rows: 1,
                                                slidesToShow: 1,
                                                slidesToScroll: 1
                                            }
                                        }
                                    ]

                                });
                            }
                        })
                    }
                }

                if (productDetailContainer.length > 0) {
                    let productId = productDetailContainer.attr("productid")

                    let viewedProducts = viewedProductStorage.getAll()

                    if (viewedProductStorage.exist(productId)) {
                        viewedProducts.splice(viewedProducts.indexOf(productId), 1)
                        viewedProducts.unshift(productId)
                    } else {
                        viewedProducts.unshift(productId)
                    }

                    if (viewedProducts.length > 10) {
                        viewedProducts.splice(10, viewedProducts.length - 1)
                    }

                    //Luu lai
                    viewedProductStorage.save(viewedProducts);
                }
            }
        }

        async function API(productIds) {
            return new Promise(async (resolve, reject) => {
                try {
                    let res = $.post("/home/products", { Ids: productIds })
                    resolve(res)
                } catch (err) {
                    reject(err)
                }
            })
        }

        function RenderViewedProducts(items) {
            let resultHTML = items.map(item => {
                return `
                        <div class="row productCard">
                            <a href="${item.url}" class="col-12 product_item_link">
                                <div class="row">
                                    <img class="product_item_img" loading="lazy" data-src="${item?.image?.url}">
                                </div>
                                <div class="row">
                                    <div class="py-3 productTitle font-weight-bold">${item.title}</div>
                                    <div class="">FROM $ ${item.price}</div>
                                </div>
                            </a>
                        </div>
                    `
            }).join('');

            settings.$recentview_productList.html(resultHTML);

            LazyLoadingImage();
        }

        return this.each(function () {
            var $this = $(this);

            cache($this);
            events();
        })
    }
})();

function SmartPhoto() {
    $(".js-img-viewer").SmartPhoto({
        arrows: true,
        nav: true,
        resizeStyle: 'fit',
    });
}

var WidgetGalleryImageHandler = (function () {
    let root = {
        el: $(".Widget_ImageGallery")
    }
    async function API(data) {
        return new Promise(async (res, resolve) => {
            try {
                let res = await $.post("/addtocart", { data: data })
                resolve(res)
            } catch (err) {
                reject(err)
            }
        });
    }
    function Cache() {
        root.$btnLoadMore = root.el.find(".btnLoadGallery");
    }
    function Events() {
        root.$btnLoadMore.click((e) => {
            let target = e.target;
            let galleryKey = $(target).attr("widgetid");
            let page = $(target).attr("page");
            let pageSize = $(target).attr("pagesize");
        })
    }
    function PreLoad() {

    }
    function Init() {
        if (root.el?.length > 0) {
            Cache();
            Events();
        }
    }
    return {
        init: Init
    }
})()