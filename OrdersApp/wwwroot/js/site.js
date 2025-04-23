$(document).ready(function () {
    $(".add-to-cart-form").submit(function (e) {
                e.preventDefault(); // otmenyaem peresylku formy

                var form = $(this);
                var data = form.serialize();

                $.ajax({
                    type: "POST",
                    url: "/Basket/AddToBasket",
                    data: data,
                    success: function () {
                        const toastEl = document.getElementById('cartToast');
                        const toast = new bootstrap.Toast(toastEl);
                        toast.show();
                        updateCartCount();
                        // mozhno zdes' obnovit' znachok korziny ili chto-to eshcho
                    },
                    error: function () {
                        alert("❌ Ошибка при добавлении в корзину.");
                    }
                });
            });
        });

function updateCartCount() {
    $.ajax({
        url: '/Basket/GetCartCount',
        method: 'GET',
        success: function (count) {
            if (count > 0) {
                $(".snapcart").show();
                $(".snapcart").text(count);
            }// obnovlyaem chislo v znachke
        },
        error: function () {
            console.log("Не удалось обновить корзину");
        }
    });
}


$(document).ready(function () {
    updateCartCount();
});


//function changeQuantity(index, delta) {
//    const quantityInput = document.getElementById(`quantity-${index}`);
//    let quantity = parseInt(quantityInput.value) || 0;
//    quantity += delta;
//    if (quantity < 1) quantity = 1;
//    quantityInput.value = quantity;

//    updateTotal();
//}

// Вызываем сразу при загрузке страницы
window.onload = function () {
    updateTotal();
};





