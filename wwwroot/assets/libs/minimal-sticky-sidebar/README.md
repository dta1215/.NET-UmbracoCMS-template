# stickySidebar
A simple and usable jQuery plugin to make a div sticky, useful for sticky sidebars that follow the user
on scroll. Heavily based on this [css-tricks post](https://css-tricks.com/scrollfollow-sidebar/).

## Usage
Simply include jQuery and the script in your head and then call it on the element you wish to make
sticky.

```html
<script>
$(function() { // document ready
    $( '.sidebar' ).stickySidebar();
});
</script>

...

<div class="sidebar">
    A nice sidebar for some important reason.
</div>
```

## Options
The current options are:

```javascript
$.fn.stickySidebar.defaults = {
    topPadding: 50,
    footerThreshold: 50,
    animationDuration: 300,
};
```

The top padding is the space down the page it should be after scrolling.
The footer threshold is length to the bottom of the page when scrolling should stop, usually the height
of your footer and any padding you require. If the sticky element is pushing your footer down, increase this
value until is does not any more.

## Example
There's an example viewable [here](https://tony-rowan.github.io/sticky-sidebar/).
