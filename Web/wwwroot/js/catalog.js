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
        price: ''
    },
    methods: {
        addToCart: function () {
            cart.addToCart({ id: this.id, name: this.name, price: this.price, qty: 1 });
        }
    },
    mounted() {
        this.id = this.$refs.product.attributes["id"].value;
        this.name = this.$refs.product.attributes["name"].value;
        this.price = this.$refs.product.attributes["price"].value;
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
            localStorage.products = JSON.stringify(this.products);
        },
        clear: function () {
            this.products = [];
            localStorage.removeItem("products");
        },
        checkout: function () {
            alert('todo');
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
                this.products = JSON.parse(localStorage.getItem('products'));
            } catch (e) {
                localStorage.removeItem('products');
            }
        }
    }
});