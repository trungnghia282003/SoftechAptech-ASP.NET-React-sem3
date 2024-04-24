import * as React from 'react'
import Label from '../../../atoms/forms/label'
import CheckRadio from '../../../atoms/forms/check-radio'

function RadioBoxList({ label, name, options, values, setFieldValue, className }) {
  return (
    <div>
      {label && <Label text={label}></Label>}
      {options.map((option) => {
        option.checked = option.value === values[name]
        option.onChange = (e) => {
          setFieldValue(name, Number(e.target.value))
        }
        return <CheckRadio name={name} {...option} className={className} />
      })}
    </div>
  )
}

export default RadioBoxList
