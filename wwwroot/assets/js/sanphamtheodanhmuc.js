
$(function () {
    SanPhamTheoDanhMucHandler.init();
})


var SanPhamTheoDanhMucHandler = (function () {
    const rootObj = {
        currPage: 1,
        pageSize: 8,
        container: $(".Widget_HienThiSanPhamTheoDanhMuc")
    }

    async function API(data) {
        Loading(true)
        return new Promise((resolve, reject) => {
            $.ajax({
                url: '/home/product_by_category',
                type: 'GET',
                data: data,
                success: function (data) {
                    Loading(false)
                    resolve(data)
                },
                error: function () {
                    Loading(false)
                    rootObj.$productList.html("Không tìm thấy kết quả nào")
                    rootObj.$productList.data("totalRows", 0)
                    reject(...arguments)
                }
            });
        })
    }

    async function GetColorList() {
        return new Promise(async (resolve, reject) => {
            try {
                let result = await $.get("/home/color_list")
                result && resolve(result)
            } catch (err) {
                reject(err)
            }     
        })
    }

    async function GetSizeList() {
        return new Promise(async (resolve, reject) => {
            try {
                let result = await $.get("/home/size_list")
                result && resolve(result)
            } catch (err) {
                reject(err)
            }
        })
    }

    async function PreLoad() {
        let data = {
            CategoryId: rootObj.$category_product_container.attr("categoryid")
        }

        CommonLoadAPI(data).then(() => {
            let totalRows = rootObj.$productList.data("totalRows")
            console.log(totalRows)
            let initDataSource = []
            for (var i = 1; i <= totalRows; i++) {
                initDataSource.push(i)
            }
            rootObj.$paginationModel = rootObj.$category_product_pagination.pagination({
                dataSource: initDataSource,
                pageSize: rootObj.$productList.data("pageSize"),
                autoHidePrevious: true,
                autoHideNext: true
            })
            rootObj.$paginationModel.addHook('afterRender', PageChange)
        })



        //Khoi tao Color list filter
        GetSizeList().then((res) => {
            if (res) {
                let resultHTML = res.map(item => {
                    return `<option value="${item.id}">${item.title}</option>`
                }).join('')

                rootObj.$sizeFilter.html(resultHTML)
            }
        }).then(() => {
            rootObj.$sizeFilter.fastselect({
                placeholder: 'Chọn kích cỡ',
                searchPlaceholder: 'Tìm kích cỡ',
                noResultsText: 'Không tìm thấy kết quả',
            });
        })

        //Khoi tao Size list filter
        GetColorList().then((res) => {
            if (res) {
                let resultHTML = res.map(item => {
                    return `<option value="${item.id}">${item.title}</option>`
                }).join('')

                rootObj.$colorFilter.html(resultHTML)
            }
        }).then(() => {
            rootObj.$colorFilter.fastselect({
                placeholder: 'Chọn loại màu',
                searchPlaceholder: 'Tìm màu',
                noResultsText: 'Không tìm thấy kết quả',
            });
        })

        let orderList = [
            {
                "name_asc": "Name ascending"
            },
            {
                "name_desc": "Name descending"
            },
            {
                "price_asc": "Price ascending"
            },
            {
                "price_desc": "Price descending"
            },
            {
                "date_asc": "Date ascending"
            },
            {
                "date_desc": "Date descending"
            }
        ]

        let select_order_productHTML = "";
        for (let i = 0; i < orderList.length; i++) {
            if (i == 0) {
                select_order_productHTML += `<option value="" selected>Select order</option>`
            } else {
                let key = Object.keys(orderList[i])[0]
                select_order_productHTML += `<option value="${key}">${orderList[i][key]}</option>`
            }
        }
        rootObj.$select_order_product.html(select_order_productHTML)


        setTimeout(() => {
            $('.is-sticky').sticksy({
                listen: false,
                topSpacing: 100
            });
        }, 2500)
    }

    function Render(res) {
        let resultHTML = res?.items?.map(item => {
            return `
                    <div class="row">
                        <a href="${item.url}" class="col-12 product_item_link">
                            <div class="row">
                                <img class="product_item_img isLoading" loading="lazy" data-src="${item.images.length > 0 && item.images[0].src}"  />
                            </div>
                            <div class="row">
                                <div class="py-3 productTitle">${item.title}</div>
                                <div class="">FROM $ ${item.price}</div>
                            </div>
                        </a>
                    </div>
                    `
        })
        //Append result HTML
        rootObj.$productList.html(resultHTML)

        LazyLoadingImage()

        $("html, body").animate({ scrollTop: rootObj.$category_product_container.offset().top - 100 }, 100, "linear")
    }

    function PageChange() {
        let currPage = rootObj.$category_product_pagination.find(".paginationjs-page.active").attr("data-num")

        let data = InitData()
        data.CurrentPage = currPage
       
        CommonLoadAPI(data)
    }

    //Đảm bảo truyền đủ data model là được
    async function CommonLoadAPI(data) {
        return new Promise(async (resolve, reject) => {
            API(data).then((res) => {
                if (res.items) {
                    rootObj.$productList.data("categoryid", rootObj.$category_product_container.attr("categoryid"))
                    rootObj.$productList.data("currentPage", res.currentPage)
                    rootObj.$productList.data("pageSize", res.pageSize)
                    rootObj.$productList.data("totalPages", res.totalPages)
                    rootObj.$productList.data("totalRows", res.totalRows)

                    Render(res)

                    resolve(res)
                }
            }).catch((err) => {
                reject(err)
            })
        })
    }

    function Cache() {
        rootObj.$category_product_container = $(".category_product_container")
        rootObj.$productList = $('.product_item_list')
        rootObj.$category_product_pagination = $(".category_product_pagination")

        rootObj.$sizeFilter = rootObj.$category_product_container.find(".select_filter_size")
        rootObj.$colorFilter = rootObj.$category_product_container.find(".select_filter_color")
        rootObj.$select_order_product = rootObj.$category_product_container.find(".select_order_product")

        rootObj.$filter_controls = rootObj.$category_product_container.find(".filter_controls")
        rootObj.$filterButton = rootObj.$filter_controls.find(".btnFilter")

        rootObj.originURL = window.location.origin + window.location.pathname
    }

    function Events() {
        //On search
        rootObj.$filterButton.click((e) => {
            let newURL = rootObj.originURL;

            if (productSizeValues) {
                newURL = newURL + `?productSize=${productSizeValues}`
            }

            if (productColorValues) {
                newURL = newURL + `&productColor=${productColorValues}`
            }
            //window.history.pushState(null, null, newURL);
        })

        rootObj.$sizeFilter.change(GetAllFilterValues)
        rootObj.$colorFilter.change(GetAllFilterValues)
        rootObj.$select_order_product.change(GetAllFilterValues)

        function GetAllFilterValues(e) {
            let data = InitData()

            CommonLoadAPI(data).finally(() => {
                let totalRows = rootObj.$productList.data("totalRows")
                let initDataSource = []
                for (var i = 1; i <= totalRows; i++) {
                    initDataSource.push(i)
                }
                rootObj.$paginationModel = rootObj.$category_product_pagination.pagination({
                    dataSource: initDataSource,
                    pageSize: rootObj.$productList.data("pageSize"),
                    autoHidePrevious: true,
                    autoHideNext: true
                })
                rootObj.$paginationModel.addHook('afterRender', PageChange)
            })
        }
    }

    function InitData() {
        let sizeFilter = rootObj.$category_product_container.find(".filter_size .fstChoiceItem")
        let colorFilter = rootObj.$category_product_container.find(".filter_color .fstChoiceItem")
        let orderFilterValue = rootObj.$select_order_product.val()

        let productSizeValues = [...sizeFilter]?.map(item => {
            let value = $(item).attr("data-value")
            return value
        }).join(',') || ""

        let productColorValues = [...colorFilter]?.map(item => {
            let value = $(item).attr("data-value")
            return value
        }).join(',') || ""

        //Khoi tao query param model
        return {
            CategoryId: rootObj.$productList.data("categoryid"),
            CurrentPage: rootObj.$productList.data("currentPage"),
            PageSize: rootObj.$productList.data("pageSize"),
            ProductSize: productSizeValues,
            ProductColor: productColorValues,
            OrderBy: orderFilterValue
        }
    }

    function Init() {
        if (rootObj.container.length > 0) {
            Cache()
            PreLoad()
            Events()
        }
    }

    return {
        init: Init
    }
})();