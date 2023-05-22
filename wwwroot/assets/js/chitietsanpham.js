$(function () {
    ChiTietSanPhamHandler.init()
})



var ChiTietSanPhamHandler = (function () {
    let root = {
        parentEl: $('.product_detail_container')
    }

    function PreLoad() {

        root.$product_images.slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            centerMode: false,
            accessibility: true,
            /*fade: true,*/
            arrows: true,
            dots: true,
            vertical: false,
            verticalSwiping: false,
            adaptiveHeight: true
        });

        root.$product_detail_sidebar.sticksy({
            listen: true,
            topSpacing: 100
        });

        root.parentEl.find(".zoom").zoom({
            on: 'mouseover',
            magnify: 1.8
        })

        //On firstload
        root.$color_items.eq(0).addClass("active")
        $.publish('OnColorActive', root.$color_items.filter(".active").attr("quantity"))
    }

    function Cache() {
        root.$product_detail_sidebar = root.parentEl.find(".product_detail_sidebar")
        root.$product_images = root.parentEl.find(".product_images")
        root.$select_size = root.parentEl.find(".select_size")
        root.$product_detail_quantity = root.parentEl.find(".product_detail_quantity")
        root.$sizes_container = root.parentEl.find(".sizes_container")
        root.$color_items = root.$sizes_container.find(".color_item")
        root.$btnAddToCart = root.parentEl.find("#btnAddToCart")
    }
    function Events() {
        root.$color_items.not(".out").click((e) => {
            let target = e.target;

            root.$color_items.removeClass("active")
            $(target).addClass("active")

            //On item active => show quantity
            let data = $(target).attr("quantity")
            $.publish("OnColorActive", data)
        })

        $.subscribe('OnColorActive', (e, data) => {
            if (data) {
                parseInt(data) == 0 ? root.$product_detail_quantity.text("Out of order")
                    : root.$product_detail_quantity.text(data)
            }
        })

        root.$btnAddToCart.click(debounce((e) => {
            let selectedItem = root.$color_items.filter(".active")
            let productId = selectedItem.attr("productid");
            let sizeId = selectedItem.attr("sizeid");
            let colorId = selectedItem.attr("colorid");

            let data = {
                ProductId: productId,
                SizeId: sizeId,
                ColorId: colorId,
                Quantity: 1
            }
            AddToCartAPI(data).then((res) => {
                $.publish("onCartChange")
            })
        }, 500))
    }

    function AddToCartAPI(data) {
        Loading(true)
        return new Promise(async (resolve, reject) => {
            try {
                let res = await $.post("/addtocart", { data: data })
                Loading(false)
                resolve(res)

            } catch (err) {
                Loading(false)
                reject(err)
            }
        })
    }

    function Init() {
        if (root.parentEl.length > 0) {
            Cache()
            Events()
            PreLoad()
        }
    }

    return {
        init: Init
    }
})()