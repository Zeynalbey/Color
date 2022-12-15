﻿let buttons = document.querySelectorAll(".add-product-to-basket-btn");

buttons.forEach(x => x.addEventListener("click", function (e) {
    e.preventDefault();
    console.log(e.target.href)
    fetch(e.target.href)
        .then(response => response.text())
        .then(data => {
            console.log(data)
            $('.cart-block').html(data);
        })
}))

$(document).on("click", ".remove-product-to-basket-btn", function (e) {
    e.preventDefault();
    fetch(e.target.href)
        .then(response => response.text())
        .then(data => {
            console.log(data)
            $('.cart-block').html(data);
        })
})