// categories
var catApp = new Vue({
    el: '#catApp',
    data: {
        categories: [],
        loaded: false,
        error: false
    },
    mounted() {
        axios.get('/api/categories')
            .then(function (r) {
                if (r && r.data) {
                    r.data.forEach(c => {
                        catApp.categories.push(c);
                        c.url = '/products/' + c.slug;
                    });
                }
            })
            .catch(function (error) {
                console.log(error);
                catApp.error = true;
            })
            .then(function () {
                catApp.loaded = true;
            });
    }
});

// products
const prodsApp = new Vue({
    el: '#prodsApp',
    data: {
        products: [],
        slug: '',
        loaded: false
    },
    methods: {
        shortDesc: function (val) {
            var desc = (val || "");
            if (desc.length < 100)
                return desc;

            return `${desc.substr(0, 100)}...`;
        }
    },
    mounted() {
        if (!this.$refs.cat)
            return;

        this.slug = this.$refs.cat.attributes["slug"].value;
        axios.get('/api/products/' + this.slug)
            .then(function (r) {
                if (r && r.data) {
                    r.data.forEach(p => {
                        prodsApp.products.push(p);
                        p.url = '/product/' + p.slug;
                    });
                }
            })
            .catch(function (error) {
                console.log(error);
                alert('Error loading the catalog. Check your log for more information');
            })
            .then(function () {
                prodsApp.loaded = true;
            });
    }
});

// product detail
var prodApp = new Vue({
    el: '#prodApp',
    data: {
        slug: '',
        name: '',
        description: '',
        price: '',
        rating: 0,
        maxRating: 5,
    },
    methods: {
        addToCart: function () {
            cart.addToCart({
                slug: this.slug,
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
        this.slug = a["slug"].value;
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
            var el = this.products.find(x => x.slug === p.slug);
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
        products: [],
        pmtId: '',
        addrId: ''
    },
    methods: {
        submit: function () {
            var li = this.products.map(p => ({
                Slug: p.slug,
                Qty: parseInt(p.qty),
                Name: p.name,
                Price: parseFloat(p.price)
            }));

            axios.post('/order/submit', {
                AddressId: this.addrId,
                PaymentId: this.pmtId,
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

            if (this.$refs.data) {
                this.addrId = this.$refs.data.attributes["addrId"].value
                this.pmtId = this.$refs.data.attributes["pmtId"].value
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
});

// recommendation
const recommApp = new Vue({
    el: '#recommApp',
    data: {
        products: [],
        hasProducts: false
    },
    mounted() {
        if (!this.$refs.recomm)
            return;

        this.slug = this.$refs.recomm.attributes["slug"].value;
        axios.get('/api/recommendations/' + this.slug)
            .then(function (r) {
                if (r && r.data && r.data.length) {
                    recommApp.hasProducts = true;
                    r.data.forEach(p => {
                        p.url = '/product/' + p.slug;
                        recommApp.products.push(p);
                    });
                }
                else {
                    recommApp.hasProducts = false;
                }
            })
            .catch(function (error) {
                console.log(error);
                recommApp.hasProducts = false;
            });
    }
});