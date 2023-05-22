$(function () {
    CartHandler.init()
})

var CartHandler = (function () {
    let root = {
        cart_panel: $("#cart_panel")
    }

    function CartsAPI() {
        return new Promise(async (resolve, reject) => {
            try {
                let res = await $.get("/carts")
                resolve(res)
            } catch (err) {
                reject(err)
            }
        })
    }
    function CartCountAPI() {
        return new Promise(async (resolve, reject) => {
            try {
                let res = await $.get("/cartcount")
                resolve(res)
            } catch (err) {
                reject(err)
            }
        })
    }
    function AddCartAPI(data) {
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
    function RemoveItemAPI(data) {
        return new Promise(async (resolve, reject) => {
            try {
                let res = await $.post("/removeitem", { data: data })
                resolve(res)
            } catch (err) {
                reject(err)
            }
        })
    }
    function LoadCartsAPI() {
        return new Promise(async (resolve, reject) => {
            try {
                let res = await $.get("/carts")
                resolve(res)
            } catch (err) {
                reject(err)
            }
        })
    }

    function Cache() {
        root.$btnOpenCartHeader = $('#btnCart')
        root.$cartCount = $("#CartCount")

        root.$cartList = root.cart_panel.find(".cart_list")
        root.$input_item_quantity = root.cart_panel.find(".input_item_quantity")

        root.$btnCheckoutCart = root.cart_panel.find("#btnCheckoutCart")
    }


    function Events() {
        //Open cart from header
        root.$btnOpenCartHeader.click((e) => {
            e.preventDefault();

            LoadCartsAPI().then((res) => {
                if (res) {
                    RenderCart(res)
                }
            })
            root.cart_panel.open()
        })


        root.cart_panel.delegate(".input_item_quantity", "change", debounce((e) => {
            let target = e.target

            if ($(target).val()) {
                let data = {
                    ProductId: $(target).attr("productId"),
                    SizeId: $(target).attr("sizeId"),
                    ColorId: $(target).attr("colorId"),
                    Quantity: $(target).val() - $(target).attr("oldQuantity")
                }
                AddCartAPI(data).then((res) => {
                    $.publish("onCartChange")
                })
            }
        }, 500))

        root.cart_panel.delegate(".btnRemoveItem", "click", (e) => {
            e.preventDefault()

            let target = e.target
            let inputTarget = $(target).closest(".cart_item").find(".input_item_quantity")

            let data = {
                ProductId: $(inputTarget).attr("productId"),
                SizeId: $(inputTarget).attr("sizeId"),
                ColorId: $(inputTarget).attr("colorId")
            }
            RemoveItemAPI(data).then(() => {
                $.publish("onCartChange")
            })
        })
    }

    function RenderCart(items) {
        let resultHTML = items.map(item => {
            return `
                <div class="row cart_item mb-4">
                    <div class="col-5 cart_image_container">
                        <img class="cart_img" src="${item.imageSrc}" />
                    </div>
                    <div class="col-7">
                        <div class="row"><label>${item.productName}</label></div>
                        <div class="row"><label>${item.sizeValue} - ${item.colorValue}</label></div>
                        <div class="row"><label>$${item.price}</label></div>
                        <div class="row d-flex align-items-center mt-4">
                            <div class="col-sm-6">
                                <input class="input_item_quantity form-control"
                                        min="1" max="100"
                                        value="${item.quantity}" 
                                        type="number"
                                        productId=${item.productId}
                                        sizeId=${item.sizeId}
                                        colorId=${item.colorId}
                                        oldQuantity=${item.quantity}
                                            />
                            </div>
                            <div class="col-sm-6">
                                <a class="btnRemoveItem">Remove</a>
                            </div>
                        </div>
                    </div>
                </div>
            `
        })
        root.$cartList.html(resultHTML)
    }

    function PreLoad() {
        root.cart_panel.SlideOutPanel({
            // settings here
            slideFrom: 'right',
            enableEscapeKey: true,
            screenClose: true,
            showScreen: true
        });

        $.subscribe("onCartChange", () => {
            LoadCartsAPI().then((res) => {
                if (res) {
                    RenderCart(res)
                }
            })
            .then(() => {
                //Cap nhat lai cart count
                CartCountAPI().then((res) => {
                    root.$cartCount.text(res)
                })
            })
        })
    }

    function Init() {
        if (root.cart_panel.length > 0) {
            Cache()
            PreLoad()
            Events()
        }
    }

    return {
        init: Init
    }
})()