﻿function updateCartCount() {
    $.ajax({
        url: '/Basket/GetCartCount',
        method: 'GET',
        success: function (count) {
            if (count > 0) {
                $(".snapcart").show();
                $(".snapcart").text(count);
            }
            else if (count <= 0) {
                $(".snapcart").hide();
            }
          
        },
        error: function () {
            console.log("Не удалось обновить корзину");
        }
    });
}


$(document).ready(function () {
    updateCartCount();
});

window.onload = function () {
    updateTotal();
};



const burger = document.getElementById('burger');
const sidebar = document.getElementById('sidebar');

burger.addEventListener('click', () => {
    burger.classList.toggle('active');
    sidebar.classList.toggle('active');
});


document.addEventListener("DOMContentLoaded", () => {
    const cartIcon = document.getElementById("cart-icon");

    document.querySelectorAll(".add-to-cart-form").forEach(form => {
        const button = form.querySelector(".add-to-cart-btn");

        const setButtonState = (added) => {
            if (added) {
                button.textContent = "Добавлено";
                button.classList.remove("btn-success");
                button.classList.add("btn-secondary");
                button.dataset.added = "true";
            } else {
                button.textContent = "В Корзину";
                button.classList.remove("btn-secondary");
                button.classList.add("btn-success");
                button.dataset.added = "false";
            }
        };

        if (button.dataset.added === "true") setButtonState(true);

        form.addEventListener("submit", function (e) {
            e.preventDefault(); // prevent form submission

            const id = parseInt(form.querySelector("input[name='id']").value);
            const name = form.querySelector("input[name='name']").value;
            const price = parseFloat(form.querySelector("input[name='price']").value);

            const isAdded = button.dataset.added === "true";

            if (isAdded) {
                $.ajax({
                    type: "POST",
                    url: "/Basket/RemoveFromCart",
                    contentType: "application/json",
                    data: JSON.stringify({ productId: id }),
                    success: function (data) {
                        if (data.success) {
                            setButtonState(false);
                            updateCartCount(); 
                        }
                    },
                    error: function () {
                        alert("❌ Ошибка при удалении из корзины.");
                    }
                });
            } else {
                const productImg = form.closest(".food-info").querySelector(".product-img");
                const imgRect = productImg.getBoundingClientRect();
                const cartRect = cartIcon.getBoundingClientRect();

                const flyingImg = productImg.cloneNode(true);
                flyingImg.style.position = "fixed";
                flyingImg.style.zIndex = "9999";
                flyingImg.style.left = imgRect.left + "px";
                flyingImg.style.top = imgRect.top + "px";
                flyingImg.style.width = imgRect.width + "px";
                flyingImg.style.height = imgRect.height + "px";
                flyingImg.style.transition = "all 1s ease-in-out";

                document.body.appendChild(flyingImg);

                setTimeout(() => {
                    flyingImg.style.left = cartRect.left + "px";
                    flyingImg.style.top = cartRect.top + "px";
                    flyingImg.style.width = "20px";
                    flyingImg.style.height = "20px";
                    flyingImg.style.opacity = "0.3";
                }, 10);

                setTimeout(() => {
                    flyingImg.remove();
                }, 1100);

                // Use AJAX for AddToBasket
                $.ajax({
                    type: "POST",
                    url: "/Basket/AddToBasket",
                    data: { id: id, name: name, price: price },
                    success: function () {
                        setButtonState(true);
                        const toastEl = document.getElementById('cartToast');
                        const toast = new bootstrap.Toast(toastEl);
                        showCartToast();
                        updateCartCount();  // Update cart item count (you should define this function)
                    },
                    error: function () {
                        alert("❌ Ошибка при добавлении в корзину.");
                    }
                });
            }
        });
    });
});

document.addEventListener("DOMContentLoaded", () => {
    const cartIcon = document.getElementById("cart-icon");

    document.querySelectorAll(".add-to-cart-form").forEach(form => {
        form.addEventListener("submit", function (e) {
            e.preventDefault();

            const button = form.querySelector(".add-to-cart-btn");
            const isAdded = button.dataset.added === "true";

            const productImg = this.closest(".food-info").querySelector(".product-img");
            const imgRect = productImg.getBoundingClientRect();
            const cartRect = cartIcon.getBoundingClientRect();

            const flyingImg = productImg.cloneNode(true);
            flyingImg.style.position = "fixed";
            flyingImg.style.zIndex = "9999";
            flyingImg.style.left = (isAdded ? cartRect.left : imgRect.left) + "px";
            flyingImg.style.top = (isAdded ? cartRect.top : imgRect.top) + "px";
            flyingImg.style.width = (isAdded ? "20px" : imgRect.width + "px");
            flyingImg.style.height = (isAdded ? "20px" : imgRect.height + "px");
            flyingImg.style.opacity = "1";
            flyingImg.style.transition = "all 1s ease-in-out";
            flyingImg.classList.add("flying-img");

            document.body.appendChild(flyingImg);

            // Анимация — зависит от cart_state (то есть data-added)
            setTimeout(() => {
                flyingImg.style.left = (isAdded ? imgRect.left : cartRect.left) + "px";
                flyingImg.style.top = (isAdded ? imgRect.top : cartRect.top) + "px";
                flyingImg.style.width = (isAdded ? imgRect.width + "px" : "20px");
                flyingImg.style.height = (isAdded ? imgRect.height + "px" : "20px");
                flyingImg.style.opacity = "0.3";
            }, 10);

            // Удаляем анимированный элемент и отправляем форму
            setTimeout(() => {
                flyingImg.remove();
            }, 1100);
        });
    });
});


function openModal() {
    const modal = document.getElementById("myModal");
    const bootstrapModal = new bootstrap.Modal(modal);
    bootstrapModal.show();
}

function closeModal() {
    const modal = document.getElementById('myModal');
    const bootstrapModal = bootstrap.Modal.getInstance(modal);
    bootstrapModal.hide();
}

//action for open comment food
function toggleCommentInput(index) {
    var commentInput = document.getElementById('comment-input-' + index);

    if (commentInput.style.maxHeight) {
        // Закрываем
        commentInput.style.maxHeight = null;
        commentInput.style.opacity = '0';
        setTimeout(() => {
            commentInput.style.display = 'none';
        }, 300); // Должно совпадать с временем transition
    } else {
        // Открываем
        commentInput.style.display = 'block';
        setTimeout(() => {
            commentInput.style.maxHeight = commentInput.scrollHeight + 'px';
            commentInput.style.opacity = '1';
        }, 10);
    }
}

const toastEl = document.getElementById('cartToast');
const toast = new bootstrap.Toast(toastEl, {
    delay: 1000, // Reduced to 1 second
    autohide: true
});
function showCartToast() {
    toast.show();
}

function setActiveCategory(element) {
    // Удаляем класс active у всех элементов
    document.querySelectorAll('#sidebar .lili a').forEach(item => {
        item.classList.remove('active');
    });

    // Добавляем класс active к выбранному элементу
    element.classList.add('active');

    // Сохраняем выбранную категорию в localStorage
    localStorage.setItem('lastActiveCategory', element.getAttribute('href'));
}

// При загрузке страницы проверяем сохраненную категорию
document.addEventListener('DOMContentLoaded', function () {
    const lastActiveCategory = localStorage.getItem('lastActiveCategory');
    if (lastActiveCategory) {
        const activeElement = document.querySelector(`#sidebar .lili a[href="${lastActiveCategory}"]`);
        if (activeElement) {
            activeElement.classList.add('active');
        }
    }
});
