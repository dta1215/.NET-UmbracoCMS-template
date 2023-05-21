$(function () {
    HeaderHandler.init();

    if (isMobileDevice()) {
        InitMobileMenu();
    }
})

function isMobileDevice() {
    return /Mobi|Android/i.test(navigator.userAgent);
}

var HeaderHandler = (function () {
    let root = {
        parent: $(".header_container")
    }

    function Cache() {
        root.$btnSearch = root.parent.find("#btnSearch")
        root.$header_search_area = root.parent.find(".header_search_area")
        root.$btnCloseSearch = root.parent.find("#btnCloseSearch")
        root.$searchInput = root.parent.find("#searchInput")
        root.$btnSearchAction = root.parent.find(".btnSearchAction")
    }

    function Events() {
        //btn search on click
        root.$btnSearch.click((e) => {
            root.$header_search_area.removeClass("active").addClass("active")
            root.parent.removeClass("search").addClass("search")

            root.$searchInput.focus()
        })

        root.$btnCloseSearch.click(() => {
            root.$header_search_area.removeClass("active")
            root.$searchInput.val("")
            root.parent.removeClass("search")
        })

        root.$btnSearchAction.click((e) => {
            Search()
        })

        root.$searchInput.keypress(Enter((e) => {
            Search()
        }))

        function Search() {
            let key = root.$searchInput.val();
            if (key.trim("").length < 1) return;

            if (key) {
                let url = `/search?key=${key}`
                window.location.href = url
            }
        }
    }

    

    async function SearchAPI() {
        return new Promise(async (resolve, reject) => {
            try {
                let res = await $.get(`/search?key=${root.$searchInput.val()}`)
                if (res) {
                    resolve(res)
                }
            } catch(err) {
                reject(err)
            }
        })
    }

    function Init() {
        Cache()
        Events()
    }

    return {
        init: Init
    }
})()


function InitMobileMenu() {
    let $header_mobile = $(".header_mobile")

    $('#mobile_nav').hcOffcanvasNav({
        disableAt: 1340,
        labelClose: 'Close',
        labelBack: 'Back',
        levelTitleAsBack: true,
        pushContent: false,
        levelOpen: 'expand',
    });

    if ($header_mobile.length) {
        $header_mobile.show();
    }

    //Events
    $("#hc-nav-1 .btnSearchAction").click((e) => {
        Search()
    })
    $("#hc-nav-1 #searchInput").keypress(Enter(() => {
        Search()
    }))

    function Search() {
        let key = $("#hc-nav-1 #searchInput").val();

        if (key.trim("").length < 1) return;

        if (key) {
            let url = `/search?key=${key}`
            window.location.href = url
        }
    }
}