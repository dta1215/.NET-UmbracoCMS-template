let existCheckoutForm = $(".checkout_container").length > 0;

$(function () {
    if (!existCheckoutForm) {
        return;
    }
    App.concern.initValidation();
})


var App = {};

App.concern = {
    "initValidation": async function () {
        let checkout_container = $(".checkout_container");
        let formEl = $("#checkoutForm");
        let btnCheckOut = $("#btnCheckOut");

        let domainLang = checkout_container.attr("lang")

        let validateModel = {
            ignore: "",
            rules: {
                'fullname': {
                    required: true,
                    maxlength: 50
                },
                'address': {
                    required: true,
                    maxlength: 100
                },
                'mobilenumber': {
                    required: true,
                    number: true,
                    minlength: 10,
                    maxlength: 10
                }
            },
            messages: {
                //'cb-tab2': { required: "Vui lòng chọn ít nhất một phương thức" },
            },
            focusInvalid: false,
            invalidHandler: function () {
                setTimeout(() => {
                    $(this).find(":input.error:first").focus();
                }, 10);
            }
        }

        formEl.validate(validateModel);

        //Form submit event
        formEl.submit((e) => {
            e.preventDefault();
        })
        //Check out form event
        btnCheckOut.click((e) => {
            let validForm = formEl.valid();

            console.log(validForm)
        })
    } 
}