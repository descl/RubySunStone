json.array!(@pois) do |poi|
  json.extract! poi, :id, :lvl, :coordX, :coordY
  json.url poi_url(poi, format: :json)
end
