// products
const prodsApp = new Vue({
    el: '#prodsApp',
    data: {
        products: [],
        catId: ''
    },
    mounted() {
        if (!this.$refs.cat)
            return;

        this.catId = this.$refs.cat.attributes["id"].value;
        axios.get('/api/products/' + this.catId)
            .then(function (r) {
                if (r && r.data) {
                    r.data.forEach(p => {
                        prodsApp.products.push(p);
                        p.url = '/products/details/' + p.id;
                    });
                }
            })
            .catch(function (error) {
                console.log(error);
            });
    }
});

// product detail
var prodApp = new Vue({
    el: '#prodApp',
    data: {
        id: '',
        name: '',
        description: '',
        price: '',
        rating: 0,
        maxRating: 5,
    },
    methods: {
        addToCart: function () {
            cart.addToCart({
                id: this.id,
                name: this.name,
                price: this.price,
                cat: this.cat,
                catName: this.catName,
                qty: 1
            });
        }
    },
    mounted() {
        if (!this.$refs.product)
            return;

        var a = this.$refs.product.attributes;
        this.id = a["id"].value;
        this.name = a["name"].value;
        this.price = a["price"].value;
        this.cat = a["cat"].value;
        this.catName = a["catName"].value;
        this.rating = parseInt(a["rating"].value);
    }
});

const cart = new Vue({
    el: '#cart',
    data: {
        products: []
    },
    methods: {
        addToCart: function (p) {
            var el = this.products.find(x => x.id === p.id);
            if (el) el.qty++;
            else this.products.push(p);
            this.save();
        },
        clear: function () {
            this.products = [];
            localStorage.removeItem("products");
        },
        remove: function (i) {
            this.products.splice(i, 1);
            this.save();
        },
        onQtyChange: function () {
            this.save();
        },
        save: function () {
            localStorage.products = JSON.stringify(this.products);
        }
    },
    computed: {
        hasItems: function () {
            return this.products.length > 0;
        },
        subtotal: function () {
            var t = 0;
            this.products.forEach(p => t += p.price * p.qty);
            return t;
        }
    },
    mounted() {
        if (localStorage.products) {
            try {
                this.products = JSON.parse(localStorage.products);
            } catch (e) {
                localStorage.removeItem('products');
            }
        }
    }
});

const cartSubmit = new Vue({
    el: '#cartSubmit',
    data: {
        products: []
    },
    methods: {
        submit: function () {
            var li = this.products.map(p => ({ Id: p.id, Qty: parseInt(p.qty), Name: p.name, Price: parseFloat(p.price) }));
            axios.post('/order/submit', {
                LineItems: li
            })
            .then(r => {
                localStorage.orderNumber = r.data;
                window.location = '/order/submitted/' + encodeURI(r.data);
            })
            .catch(error => {
                alert('Error submitting, please check your log');
                console.log(error);
            });
        }
    },
    mounted() {
        if (localStorage.products) {
            try {
                this.products = JSON.parse(localStorage.products);
            } catch (e) {
                localStorage.removeItem('products');
            }
        }
    }
});

const orderSubmitted = new Vue({
    el: '#orderInfo',
    data: {
        orderNumber: ''
    },
    mounted() {
        this.orderNumber = localStorage.orderNumber;
        if (this.$refs.clearCart) {
            localStorage.clear();
        }
    }
})