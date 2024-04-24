import _ from 'lodash'
import { React, useState, useContext, useEffect } from 'react'
import { AuthContext } from '../../contexts/AuthContext'
import ItemApi from '../../api/item'
import Template from '../templates/default/no-separation-template'
import { useNavigate, useSearchParams } from 'react-router-dom'
import ViewItemDetailLabel from './item-detail-label'
import './search-results.scss'
import ProtectedRoute from '../protected-route'

const SearchResult = () => {
  const [category, setCategory] = useState(null)
  const navigate = useNavigate()
  let [searchParams, setSearchParams] = useSearchParams()
  const keyWord = searchParams.get('keyword')
  const { currentUser } = useContext(AuthContext)
  let token = null
  if (currentUser) {
    token = currentUser.token
  }

  useEffect(() => {
    if (token) {
      const itemSreaching = async () => {
        if (keyWord) {
          const items = await ItemApi.searchItem(keyWord, token)
          if (items.length != 0) {
            setCategory(items)
            return
          }
        }
        setCategory(null)
      }
      itemSreaching()
    }
  }, [token, keyWord])

  const content = (
    <div className='search-results-items'>
      {category !== null ? <ViewItemDetailLabel items={category}></ViewItemDetailLabel> : navigate('/')}
    </div>
  )

  return (
    <>
      <ProtectedRoute />
      <Template headerIcon={'images/search.svg'} headerTitle={'Search Results'} content={content} />
    </>
  )
}
export default SearchResult
