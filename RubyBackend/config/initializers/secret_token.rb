# Be sure to restart your server when you modify this file.

# Your secret key is used for verifying the integrity of signed cookies.
# If you change this key, all old signed cookies will become invalid!

# Make sure the secret is at least 30 characters and all random,
# no regular words or you'll be exposed to dictionary attacks.
# You can use `rake secret` to generate a secure secret key.

# Make sure your secret_key_base is kept private
# if you're sharing your code publicly.
RubyBackend::Application.config.secret_key_base = 'eb611807bc838111f06eafcf91ba04797d01aab53818ce142bd7c047c6e21e96cd8ed1e381babac407b1f9280cd20bb944003969bd4039e0506adbf8899c87e2'
