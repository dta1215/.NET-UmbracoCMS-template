;(function ($) {

    $.fn.Tab = function (options) {
        var containerElement = this

        var defaultObject = {
            selectedIndex: 0
        }
        $.extend(defaultObject, options)

        var handler = {
            cacheDom: function () {
                this.$parent = containerElement
                this.$controls = this.$parent.find(".tab_control_child")
                this.$content = this.$parent.find(".tab_content_child")
            },
            render: function () {
                $(this.$content).hide()
                $(this.$content).filter(".active").show()
            },
            activeIndex: function (i) {
                $(this.$controls).removeClass("active")
                $(this.$controls).eq(i).addClass("active")

                $(this.$content).removeClass("active")
                $(this.$content).eq(i).addClass("active")

                this.render()
            },
            events: function () {
                $(this.$controls).bind("click", this.ChangeTab)
            },
            ChangeTab: function (e) {
                let index = $(handler.$controls).index(this)
                handler.activeIndex(index)
            },
            init: function () {
                this.cacheDom()
                this.events()

                let existIndex = $(".tab_control_child.active").index()

                if (existIndex < 0) {
                    this.activeIndex(defaultObject.selectedIndex)
                } else {
                    this.activeIndex(existIndex)
                }
            }
        }

        handler.init()

        return handler
    }

})(jQuery)

$(function () {
    TabContent.init()
})

var TabContent = (function () {

    async function GetArticleByCategoryId(categoryId, page = 1) {
        let result = await $.get(`/getarticlebycategory/${categoryId}/${page}`)
        return result
    }

    function RenderArticleByCategory(articles) {
        if (articles) {
            let html = articles?.map((article) => {
                return `
                    
                `
            })
        }
    }

    function Init() {
    }

    return {
        init: Init
    }
})()