json.array!(@maps) do |map|
  json.extract! map, 
  json.url map_url(map, format: :json)
end
