json.array!(@pois) do |poi|
  json.extract! poi, :x, :y, :type, :lvl
  json.url poi_url(poi, format: :json)
end
